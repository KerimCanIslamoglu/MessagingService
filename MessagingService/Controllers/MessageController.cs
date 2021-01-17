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
    public class MessageController : ControllerBase
    {
        private IMessageService _messageService;
        private IBlockedUserService _blockedUserService;
        private IErrorLogsService _errorLogsService;
        private UserManager<ApplicationUser> _userManager;

        public MessageController(IMessageService messageService, IBlockedUserService blockedUserService, UserManager<ApplicationUser> userManager, IErrorLogsService errorLogsService)
        {
            _messageService = messageService;
            _blockedUserService = blockedUserService;
            _errorLogsService = errorLogsService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/[controller]/GetOldMessages")]
        public IActionResult GetOldMessages(string userToName)
        {
            try
            {
                var userFromName = User.Identity.Name;
                var oldMessages=_messageService.GetOldMessages(userFromName, userToName).Select(x => new MessageModel
                {
                    MessageDetail = x.MessageDetail,
                    UserFrom = x.UserFrom,
                    UserTo = x.UserTo,
                    SendedAt = x.SendedAt
                }).ToList();

                string message = string.Empty;
                if (oldMessages != null)
                    message = "Mesaj geçmişiniz başarıyla getirildi.";
                else
                    message = "Bu kullanıcıya ait herhangi bir mesaj bulunamamıştır.";

                return Ok(new ResponseListModel<MessageModel>
                {
                    Success = true,
                    Message = message,
                    Response = oldMessages
                });
            }
            catch (Exception ex)
            {
                AddErrorLog(ex,"GetOldMessages");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseListModel<MessageModel>
                {
                    Success = false,
                    Message = "Mesaj geçmişi getirilirken bir hata oluştu.",
                    Response = null
                });
            }

        }

        [HttpPost]
        [Route("api/[controller]/SendMessage")]
        public async Task<IActionResult> SendMessage(SendMessageModel sendMessageModel)
        {
            try
            {
                bool success;
                string returnMessage = string.Empty;

                var user = await _userManager.FindByNameAsync(sendMessageModel.UserTo);

                if (user != null)
                {
                    var userFrom = User.Identity.Name;

                    var isBlockedUser = _blockedUserService.GetBlockedUserByName(sendMessageModel.UserTo, userFrom).Any();

                    if (isBlockedUser)
                    {
                        success = false;
                        returnMessage = "Mesaj atmak istediğiniz kullanıcı sizi bloklamış.";
                    }
                    else
                    {
                        var message = new Message
                        {
                            MessageDetail = sendMessageModel.Message,
                            SendedAt = DateTime.Now,
                            UserTo = sendMessageModel.UserTo,
                            UserFrom = userFrom
                        };

                        _messageService.SendMessage(message);

                        success = true;
                        returnMessage = "Mesajınız başarıyla gönderilmiştir";
                    }
                }
                else
                {
                    success = false;
                    returnMessage = "Mesaj göndermek istediğiniz kullanıcı adı mevcut değil.";
                }

                return Ok(new ResponseObjectModel<SendMessageModel>
                {
                    Success = success,
                    Message = returnMessage,
                    Response = null
                });
            }
            catch (Exception ex)
            {
                AddErrorLog(ex, "SendMessage");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObjectModel<SendMessageModel>
                {
                    Success = false,
                    Message = "Mesaj göndermek istediğiniz kullanıcı adı mevcut değil.",
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