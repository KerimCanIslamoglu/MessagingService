using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Identity;
using MessagingService.Entities.Entities;
using MessagingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Controllers
{
    
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UserManagementController : ControllerBase
    {
        private IBlockedUserService _blockedUserService;
        private UserManager<ApplicationUser> _userManager;
        private IErrorLogsService _errorLogsService;

        public UserManagementController(IBlockedUserService blockedUserService, UserManager<ApplicationUser> userManager, IErrorLogsService errorLogsService)
        {
            _blockedUserService = blockedUserService;
            _userManager = userManager;
            _errorLogsService = errorLogsService;
        }
        [HttpPost]
        [Route("api/[controller]/BlockUser")]
        public async Task<IActionResult> BlockUser(BlockedUserModel blockedUserModel)
        {
            try
            {
                bool success;
                string message = string.Empty;

                var blockingUserName = User.Identity.Name;
                var blockedUser = new BlockedUser
                {
                    BlockedUserName = blockedUserModel.BlockedUserName,
                    BlockingUserName = blockingUserName,
                    BlockedAt = DateTime.Now
                };

                bool isBlockedBefore = _blockedUserService.GetBlockedUserByName(blockingUserName, blockedUserModel.BlockedUserName).Any();

                if (isBlockedBefore)
                {
                    success = false;
                    message = blockedUserModel.BlockedUserName + " kullanıcı daha önce bloklanmış.";
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(blockedUserModel.BlockedUserName);

                    if (user==null)
                    {
                        success = false;
                        message = blockedUserModel.BlockedUserName + " kullanıcı bulunamadı.";
                    }
                    else
                    {
                        _blockedUserService.BlockUser(blockedUser);

                        success = true;
                        message = blockedUserModel.BlockedUserName + " adlı kullanıcı başarıyla bloklanmıştır";
                    }
                }

                return Ok(new ResponseObjectModel<BlockedUserModel>
                {
                    Success = success,
                    Message = message,
                    Response = null
                });
            }
            catch (Exception ex)
            {
                AddErrorLog(ex, "BlockUser");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObjectModel<BlockedUserModel>
                {
                    Success = false,
                    Message = "Kullanıcı bloklanırken bir hata oluştu.",
                    Response = null
                });
            }
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