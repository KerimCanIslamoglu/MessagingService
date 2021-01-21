using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MessagingService.Test
{
    [TestFixture]
    public class MessageTest
    {
        Mock<IMessageDal> _mockMessageDal;

        public MessageTest()
        {
            var messageList = new List<Message>
            {
                new Message {Id=1,MessageDetail="TestDetail1",SendedAt=DateTime.Now,UserFrom="Test1",UserTo="Test2"},
            };

            var mockMessageDal = new Mock<IMessageDal>();

            mockMessageDal.Setup(mr => mr.GetAll(null)).Returns(messageList);

            mockMessageDal.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int i) => messageList.SingleOrDefault(x => x.Id == i));

            mockMessageDal.Setup(mr => mr.GetOldMessagesByUserToName(It.IsAny<string>(), It.IsAny<string>())).Returns(messageList);

            mockMessageDal.Setup(mr => mr.Create(It.IsAny<Message>())).Callback(
            (Message target) =>
            {
                messageList.Add(target);
            });

            mockMessageDal.Setup(mr => mr.Update(It.IsAny<Message>())).Callback(
                (Message target) =>
                {
                    var original = messageList.Where(q => q.Id == target.Id).Single();

                    if (original == null)
                    {
                        throw new InvalidOperationException();
                    }

                    original.UserFrom = target.UserFrom;
                    original.UserTo = target.UserTo;
                    original.MessageDetail = target.MessageDetail;

                });


            _mockMessageDal = mockMessageDal;
        }

        [Test]
        public void MessageService_GetOldMessages_ReturnsMessageList()
        {
            var actualOldMessage = new List<Message>
            {
                new Message{Id=1,MessageDetail="TestDetail1",SendedAt=DateTime.Now,UserFrom="Test1",UserTo="Test2"}
            };

            var oldMessages = _mockMessageDal.Object.GetOldMessagesByUserToName("Test1", "Test2");
            Assert.IsNotNull(oldMessages);
            Assert.IsTrue(oldMessages.Count > 0);
            Assert.AreEqual(oldMessages.Count > 0, actualOldMessage.Count > 0);
        }

        [Test]
        public void MessageService_SendMessage_ShouldSendMessage()
        {
            var message = new Message 
            {
                Id = 2, 
                MessageDetail = "TestDetail2", 
                SendedAt = DateTime.Now,
                UserFrom = "Test3", 
                UserTo = "Test4" 
            };

            _mockMessageDal.Object.Create(message);

            var expected = _mockMessageDal.Object.GetById(2);

            Assert.AreEqual(expected.Id, 2);
            Assert.AreEqual(expected.MessageDetail, "TestDetail2");
            Assert.AreEqual(expected.UserFrom, "Test3");
            Assert.AreEqual(expected.UserTo, "Test4");
            Assert.IsNotNull(expected);
        }

        [Test]
        public void MessageService_GetAll_ReturnsCount()
        {
            var expected = _mockMessageDal.Object.GetAll().Count;

            Assert.IsNotNull(expected);
            Assert.IsTrue(expected > 0);
        }

        [Test]
        public void MessageService_Update_ReturnsUpdatedValues()
        {
            var actual = new Message { Id = 1, MessageDetail = "TestDetail1_Updated", SendedAt = DateTime.Now, UserFrom = "Test1_Updated", UserTo = "Test2_Updated" };

            _mockMessageDal.Object.Update(actual);

            var expected = _mockMessageDal.Object.GetById(actual.Id);

            Assert.IsNotNull(expected);
            Assert.AreEqual(actual.UserFrom, expected.UserFrom);
            Assert.AreEqual(actual.UserTo, expected.UserTo);
            Assert.AreEqual(actual.MessageDetail, expected.MessageDetail);
        }

        [Test]
        public void MessageService_GetMessageById_ShouldNotReturnNull()
        {
            var expected = _mockMessageDal.Object.GetById(1);
           
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.UserFrom, "Test1");
            Assert.AreEqual(expected.UserTo, "Test2");
            Assert.AreEqual(expected.MessageDetail, "TestDetail1");
        }

        [TestCase(3,ExpectedResult =null)]
        [TestCase(4,ExpectedResult =null)]
        [TestCase(5,ExpectedResult =null)]
        [Test]
        public Message MessageService_GetMessageByWrongId_ShouldReturnNull(int id)
        {
            return _mockMessageDal.Object.GetById(id);
        }
    }
}
