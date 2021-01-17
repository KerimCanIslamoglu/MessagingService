using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Abstract
{
    public interface IMessageService
    {
        List<Message> GetOldMessages(string userFromName,string userToName);
        void SendMessage(Message message);
    }
}
