using Application.Abstraction.AbsToken;
using Application.ViewModel;
using Domain.Entities.user;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using UserManagement.Presentation.shared;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;


        public UsersController(UserManager<ApplicationUser> userManager, ITokenHandler tokenHandler ,RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;


        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var userViewModels = users.Select(user => new GetUsersViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    image = user.image,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                }).ToList();

                return Ok(userViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOnlyActiveUsers()
        {
            try
            {
                var users = _userManager.Users.Where(p => p.IsDeleted == false).ToList();
                var userViewModels = users.Select(user => new GetUsersViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    image = user.image,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                }).ToList();

                return Ok(userViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOnlyStudents()
        {
            try
            {
                var users = _userManager.Users.Where(p => p.IsDeleted == false).ToList();

                var studentUsers = new List<GetUsersViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("student"))
                    {
                        var userViewModel = new GetUsersViewModel
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            email = user.Email,
                            phoneNumber = user.PhoneNumber,
                            field = user.field
                        };
                        studentUsers.Add(userViewModel);
                    }
                }

                return Ok(studentUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOnlyTeachers()
        {
            try
            {
                var users = _userManager.Users.Where(p => p.IsDeleted == false).ToList();

                var studentUsers = new List<GetUsersViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("teacher"))
                    {
                        var userViewModel = new GetUsersViewModel
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            image = user.image,
                            email = user.Email,
                            phoneNumber = user.PhoneNumber,
                            field = user.field
                        };
                        studentUsers.Add(userViewModel);
                    }
                }

                return Ok(studentUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            try
            {
                var users = _userManager.Users.Where(p => p.Id == id && !p.IsDeleted).ToList();

                if (users.Count > 0)
                {
                    var userViewModels = users.Select(user => new GetUsersViewModel
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        image = user.image,
                        email = user.Email,
                        phoneNumber = user.PhoneNumber,

                    }).ToList();


                    return Ok(userViewModels);
                }
                else
                {
                    return BadRequest("Bu Kullanıcı Bulunamadı, Silinmiş Olabilir.");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetOnlyStudentsByLetter(GetUserByLetter model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.letter))
                {
                    return BadRequest("Geçersiz giriş. Lütfen bir kullanıcı adı parçası girin.");
                }

                var users = _userManager.Users
                    .Where(p => p.IsDeleted == false && p.UserName.Contains(model.letter))
                    .ToList();

                var filteredStudents = new List<GetUsersViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("student"))
                    {
                        var userViewModel = new GetUsersViewModel
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            email = user.Email,
                            phoneNumber = user.PhoneNumber,
                            field = user.field
                        };
                        filteredStudents.Add(userViewModel);
                    }
                }

                return Ok(filteredStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetOnlyTeachersByLetter(GetUserByLetter model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.letter))
                {
                    return BadRequest("Geçersiz giriş. Lütfen bir kullanıcı adı parçası girin.");
                }

                var users = _userManager.Users
                    .Where(p => p.IsDeleted == false && p.UserName.Contains(model.letter))
                    .ToList();

                var filteredTeachers = new List<GetUsersViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("teacher"))
                    {
                        var userViewModel = new GetUsersViewModel
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            email = user.Email,
                            phoneNumber = user.PhoneNumber,
                            field = user.field
                        };
                        filteredTeachers.Add(userViewModel);
                    }
                }

                return Ok(filteredTeachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

       

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateStudentUser(CreateUserModel model)
        {
                var user = new ApplicationUser {

                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.email,
                IsDeleted = false,
                image = "",
                PhoneNumber = model.phoneNumber,
                field = model.field,
             };

            if (model.password == model.passwordConfirm)
            {
                IdentityResult result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "student");
                    return Ok("Başarılı");
                }
                else
                    return Ok(result.Errors.First().Description);
            }
            else
            {
                return BadRequest("Şifreler eşleşmiyor");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTeacherUser(CreateUserModel model)
        {
            var user = new ApplicationUser
            {

                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.email,
                field = model.field,
                IsDeleted = false,
                image = "",
                PhoneNumber = model.phoneNumber
            };

            if (model.password == model.passwordConfirm)
            {
                IdentityResult result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "teacher");
                    return Ok("Başarılı");
                }
                else
                    return Ok(result.Errors.First().Description);
            }
            else
            {
                return BadRequest("Şifreler eşleşmiyor");
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> createAdmin(CreateUserModel model)
        {

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.email,
                IsDeleted = false,
                PhoneNumber = model.phoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.password);



            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok("Başarılı");
            }
            else
                return Ok(result.Errors.First().Description);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> createRole( CreateRoleModels model)
        {
            IdentityResult result = await _roleManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.name

            }) ;

            if (result.Succeeded)
                return Ok("Başarılı");
            else
                return Ok(result.Errors.First().Description);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> login(Application.ViewModel.LoginUser model)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(model.userNameorEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.userNameorEmail);
                if (user == null)
                {
                    return BadRequest("Kullanıcı Adı veya Email hatalı..");
                }
            }


            if (user.IsDeleted == true)
            {
                return BadRequest("Bu Kullanıcı Silinmiş");
            }


            bool result = await _userManager.CheckPasswordAsync(user, model.password);

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var role = roles.FirstOrDefault();



            Application.DTOs.Token token = new();

            if (role == "Admin")
            {
                token = _tokenHandler.createAccessToken(45, _configuration["Token:SecurityKey"], _configuration["Token:Audience"], _configuration["Token:Issuer"], user , role);
            }
            else
            {
                token = _tokenHandler.createAccessToken(45, _configuration["userToken:SecurityKey"], _configuration["userToken:Audience"], _configuration["userToken:Issuer"], user, role);
            }

            if (result)
                return Ok(token);
            else
                return BadRequest("Şifre Hatalı");

        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeInfos([FromBody] UpdateUsersInfosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Alanları Doğru Bir Şekilde Doldurun.");
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null )
            {
                return NotFound("User not found");
            }


            if (!string.IsNullOrEmpty(model.email) && model.email.Contains("@") && model.email.Contains(".com"))
            {
                user.Email = model.email;
                user.EmailConfirmed = false;
            }
           

            if (!string.IsNullOrEmpty(model.Username) && model.Username.Length > 2)
            {
                user.UserName = model.Username;
            }

            if (!string.IsNullOrEmpty(model.phoneNumber) && model.phoneNumber.Length > 2)
            {
                user.PhoneNumber = model.phoneNumber;
            }

            if (!string.IsNullOrEmpty(model.field) && model.field.Length > 2)
            {
                user.field = model.field;
            }

            if (!string.IsNullOrEmpty(model.image))
            {
                user.image = model.image;
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Başarılı");
            }
            else
            {
                return BadRequest(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }

        }
       

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateUserPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!string.IsNullOrEmpty(model.newPassword))
            {
                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    return BadRequest(string.Join(",asdasd ", passwordChangeResult.Errors.Select(e => e.Description)));
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Başarılı");
            }
            else
            {
                return BadRequest(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }

        }

        [HttpPut("[action]")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.IsDeleted = true;


            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Başarılı");
            }
            else
            {
                return BadRequest(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }

        }

    }
}
