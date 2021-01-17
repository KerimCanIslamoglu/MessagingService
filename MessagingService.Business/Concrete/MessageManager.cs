using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Concrete
{
    public class MessageManager : IMessageService
    {
        private IMessageDal _messageDal;
        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }
        public List<Message> GetOldMessages(string userFromName, string userToName)
        {
            return _messageDal.GetOldMessagesByUserToName(userFromName,userToName);
        }

        public void SendMessage(Message message)
        {
            _messageDal.Create(message);
        }
    }
}
