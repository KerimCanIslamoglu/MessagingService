using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessagingService.Test
{
    [TestFixture]
    public class UserLogTest
    {
        Mock<IUserLogDal> _mockUserLogDal;

        public UserLogTest()
        {
            var userLogList = new List<UserLog>
            {
                new UserLog {Id=1,CreatedAt=DateTime.Now,Activity="TestActivity1",ActivityDescription="TestDescription1",UserName="Test1"},
            };

            var mockUserLogDal = new Mock<IUserLogDal>();

            mockUserLogDal.Setup(mr => mr.GetAll(null)).Returns(userLogList);

            mockUserLogDal.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int i) => userLogList.SingleOrDefault(x => x.Id == i));

            mockUserLogDal.Setup(mr => mr.Create(It.IsAny<UserLog>())).Callback(
               (UserLog target) =>
               {
                   userLogList.Add(target);
               });

            mockUserLogDal.Setup(mr => mr.Update(It.IsAny<UserLog>())).Callback(
                (UserLog target) =>
                {
                    var original = userLogList.Where(q => q.Id == target.Id).Single();

                    if (original == null)
                    {
                        throw new InvalidOperationException();
                    }

                    original.Activity = target.Activity;
                    original.ActivityDescription = target.ActivityDescription;
                    original.UserName = target.UserName;

                });


            _mockUserLogDal = mockUserLogDal;
        }

        [Test]
        public void UserLog_Insert_AddingUserLog()
        {
            var userLog = new UserLog
            {
                Id = 2,
                CreatedAt = DateTime.Now,
                Activity = "TestActivity2",
                ActivityDescription = "TestDescription2",
                UserName = "Test2",
            };
            _mockUserLogDal.Object.Create(userLog);

            var expected = _mockUserLogDal.Object.GetById(2);

            Assert.AreEqual(expected.Id, 2);
            Assert.AreEqual(expected.Activity, "TestActivity2");
            Assert.AreEqual(expected.ActivityDescription, "TestDescription2");
            Assert.AreEqual(expected.UserName, "Test2");
            Assert.IsNotNull(expected);
        }

        [Test]
        public void UserLog_GetAll_ReturnsCount()
        {
            var expected = _mockUserLogDal.Object.GetAll().Count;

            Assert.IsNotNull(expected);
            Assert.IsTrue(expected > 0);
        }

        [Test]
        public void UserLog_Update_ReturnsUpdatedValues()
        {
            var actual = new UserLog { Id = 1, Activity = "TestActivity2_Updated", ActivityDescription = "TestDescription2_Updated", UserName = "Test2_Updated", CreatedAt = DateTime.Now };

            _mockUserLogDal.Object.Update(actual);

            var expected = _mockUserLogDal.Object.GetById(actual.Id);

            Assert.IsNotNull(expected);
            Assert.AreEqual(actual.Activity, expected.Activity);
            Assert.AreEqual(actual.ActivityDescription, expected.ActivityDescription);
            Assert.AreEqual(actual.UserName, expected.UserName);
        }

        [Test]
        public void UserLog_GetUserLogById_ShouldNotReturnNull()
        {
            var expected = _mockUserLogDal.Object.GetById(1);

            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.Activity, "TestActivity1");
            Assert.AreEqual(expected.ActivityDescription, "TestDescription1");
            Assert.AreEqual(expected.UserName, "Test1");
        }

        [TestCase(3, ExpectedResult = null)]
        [TestCase(4, ExpectedResult = null)]
        [TestCase(5, ExpectedResult = null)]
        [Test]
        public UserLog UserLog_GetUserLogByWrongId_ShouldReturnNull(int id)
        {
            return _mockUserLogDal.Object.GetById(id);
        }
    }
}
