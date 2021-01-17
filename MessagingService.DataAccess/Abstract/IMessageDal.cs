using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.DataAccess.Abstract
{
    public interface IMessageDal:IRepositoryBase<Message>
    {
        List<Message> GetOldMessagesByUserToName(string userFromName, string userToName);
    }
}
