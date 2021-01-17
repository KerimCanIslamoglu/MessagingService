using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Concrete
{
    public class BlockedUserManager : IBlockedUserService
    {
        private IBlockedUserDal _blockedUserDal;

        public BlockedUserManager(IBlockedUserDal blockedUserDal)
        {
            _blockedUserDal = blockedUserDal;
        }
        public void BlockUser(BlockedUser blockedUser)
        {
            _blockedUserDal.Create(blockedUser);
        }

        public List<BlockedUser> GetBlockedUserByName(string blockingUserName, string blockedUserName)
        {
            return _blockedUserDal.GetBlockedUserByName(blockingUserName, blockedUserName);
        }
    }
}
