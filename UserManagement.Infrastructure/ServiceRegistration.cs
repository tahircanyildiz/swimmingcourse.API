using Application.Abstraction.AbsToken;
using Application.Abstraction.Services.MailService;
using Blog.Infrastructure.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.Concrete;

namespace UserManagement.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddScoped<ITokenHandler, TokenHandler>();
            service.AddScoped<IMailService, MailService>();
        }

    }
}
