using Domain.Entities.user;
using Microsoft.AspNetCore.Identity;
using UserManagement.Persistence;
using UserManagement.Persistence.context;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

//CORS
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

//Serilog
Logger log = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt")
    .Enrich.FromLogContext()// bu kod ile log contexte middleware ile sonradan ekledi�imiz propertylerdeki de�erlere ula��yoruz.
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});


builder.Services.AddControllers();

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Kullan�c� ad� i�in �zel do�rulama kurallar�n� burada belirleyin
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789������������ ";
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
    


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// gizli bilgileri tutmak ama�l�
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddJsonFile("secrets.json", optional: true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,// hangi sitelerin kullanac��n� belirledi�imiz alan www.bilmemne
            ValidateIssuer = true,  // olu�turulacak token � kimin dap�tt���n� belirledi�imiz alan API k�sm� myapi.com
            ValidateLifetime = true, // token s�resini kontrol etti�imiz yer
            ValidateIssuerSigningKey = true, // �retilecek token de�erinin uygulamam�za ait oldu�unu belirledi�imiz alan security key verisinin do�rulanmas�

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            // �retilen token�n expire s�resini ayarl�yoruz
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name // jwt �zerinde Name claimne kar��l�k gelen de�eri User.Identity.Name properrtysnden elde edebiliriz.
        };

    }

)
     .AddJwtBearer("student", options =>
     {
         options.TokenValidationParameters = new()
         {
             ValidateAudience = true,
             ValidateIssuer = true,  // olu�turulacak token � kimin dap�tt���n� belirledi�imiz alan API k�sm� myapi.com
             ValidateLifetime = true, // token s�resini kontrol etti�imiz yer
             ValidateIssuerSigningKey = true, // �retilecek token de�erinin uygulamam�za ait oldu�unu belirledi�imiz alan security key verisinin do�rulanmas�

             ValidAudience = builder.Configuration["userToken:Audience"],
             ValidIssuer = builder.Configuration["userToken:Issuer"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["userToken:SecurityKey"])),
             // �retilen token�n expire s�resini ayarl�yoruz
             LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

             NameClaimType = ClaimTypes.NameIdentifier, // jwt �zerinde Name claimne kar��l�k gelen de�eri User.Identity.Name properrtysnden elde edebiliriz.
             

         };

     }
     );

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


//bu middleware kendinden sonrakileri loglat�r �ncekiler loglatmaz
app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
