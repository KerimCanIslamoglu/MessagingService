using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Abstract
{
    public interface IUserLogService
    {
        void AddUserLog(UserLog userLog);
    }
}
