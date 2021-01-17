using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Abstract
{
    public interface IBlockedUserService
    {
        void BlockUser(BlockedUser blockedUser);
        List<BlockedUser> GetBlockedUserByName(string blockingUserName, string blockedUserName);

    }
}
