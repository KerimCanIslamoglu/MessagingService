using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessagingService.DataAccess.Concrete
{
    public class MessageDal:GenericRepository<Message,ApplicationDbContext>,IMessageDal
    {
        private ApplicationDbContext _dbContext;
        public MessageDal(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Message> GetOldMessagesByUserToName(string userFromName, string userToName)
        {
            return _dbContext.Messages.Where(x => x.UserTo == userToName&&x.UserFrom==userFromName).ToList();
        }
    }
}
