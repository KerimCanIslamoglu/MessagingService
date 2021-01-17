using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Identity;
using MessagingService.Entities.Entities;
using MessagingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MessagingService.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private IUserLogService _userLogService;
        private IErrorLogsService _errorLogsService;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserLogService userLogService, IErrorLogsService errorLogsService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userLogService = userLogService;
            _errorLogsService = errorLogsService;
        }

        [HttpPost]
        [Route("api/[controller]/Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            try
            {
                //Test1 Test@1
                //Test2 Test@2
                //Test123   Test@123
                string activityDescription;
                string activity = "Login";
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    activityDescription = "Kullanıcı başarıyla giriş yapabildi.";
                    AddUserLog(model.Username, activityDescription, activity);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                activityDescription = "Kullanıcı adı-şifre hatalı";
                AddUserLog(model.Username, activityDescription, activity);

                return Unauthorized();
            }
            catch (Exception ex)
            {
                AddErrorLog(ex, "Login");

                return StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponse { Status = "Error", Message = "Kullanıcı girişi yapılırken beklenmedik bir hata oluştu!" });
            }
        }

        [HttpPost]
        [Route("api/[controller]/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                string activityDescription;
                string activity = "Register";

                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    activityDescription = "Bu kullanıcı adı daha önce kayıt olmuş";
                    AddUserLog(model.Username, activityDescription, activity);

                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponse { Status = "Error", Message = "Bu kullanıcı mevcut!" });
                }

                ApplicationUser user = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    activityDescription = "Kullanıcı oluşturulurken hata oluştu!";
                    AddUserLog(model.Username, activityDescription, activity);

                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponse { Status = "Error", Message = "Kullanıcı oluşturulurken hata oluştu!" });
                }

                activityDescription = "Kullanıcı başarıyla oluşturuldu!";
                AddUserLog(model.Username, activityDescription, activity);

                return Ok(new IdentityResponse { Status = "Success", Message = "Kullanıcı başarıyla oluşturuldu!" });
            }
            catch (Exception ex)
            {
                AddErrorLog(ex, "Register");

                return StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponse { Status = "Error", Message = "Kullanıcı oluşturulurken beklenmedik bir hata oluştu!" });
            }
           
        }

        private void AddUserLog(string userName, string activityDescription,string activity)
        {
            var userLog = new UserLog();

            userLog.Activity = activity;
            userLog.ActivityDescription = activityDescription;
            userLog.CreatedAt = DateTime.Now;
            userLog.UserName = userName;

            _userLogService.AddUserLog(userLog);
        }

        private void AddErrorLog(Exception ex, string methodName)
        {
            var errorLog = new ErrorLogs()
            {
                CreatedAt = DateTime.Now,
                ErrorDetail = ex.ToString(),
                MethodName = methodName,
                UserName = User.Identity.Name
            };

            _errorLogsService.AddErrorLog(errorLog);
        }
    }
}