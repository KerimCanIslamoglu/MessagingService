using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.DataAccess.Abstract
{
    public interface IBlockedUserDal : IRepositoryBase<BlockedUser>
    {
        List<BlockedUser> GetBlockedUserByName(string blockingUserName, string blockedUserName);

    }
}
