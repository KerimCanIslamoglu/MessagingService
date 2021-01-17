using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.DataAccess.Concrete
{
    public class UserLogDal:GenericRepository<UserLog,ApplicationDbContext>,IUserLogDal
    {
    }
}
