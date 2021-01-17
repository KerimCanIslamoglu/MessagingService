using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessagingService.DataAccess.Concrete
{
    public class BlockedUserDal:GenericRepository<BlockedUser, ApplicationDbContext>,IBlockedUserDal
    {
        private ApplicationDbContext _dbContext;
        public BlockedUserDal(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BlockedUser> GetBlockedUserByName(string blockingUserName, string blockedUserName)
        {
            return _dbContext.BlockedUsers.Where(x => x.BlockedUserName == blockedUserName && x.BlockingUserName == blockingUserName).ToList();
        }
    }
}
