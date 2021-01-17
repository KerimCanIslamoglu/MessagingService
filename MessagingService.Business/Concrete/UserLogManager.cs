using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Concrete
{
    public class UserLogManager : IUserLogService
    {
        private IUserLogDal _userLogDal;

        public UserLogManager(IUserLogDal userLogDal)
        {
            _userLogDal = userLogDal;
        }

        public void AddUserLog(UserLog userLog)
        {
            _userLogDal.Create(userLog);
        }
    }
}
