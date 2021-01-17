using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessagingService.Business.Abstract;
using MessagingService.Business.Concrete;
using MessagingService.DataAccess.Abstract;
using MessagingService.DataAccess.Identity;
using MessagingService.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace MessagingService.Test
{
    [TestFixture]
    public class UserManagementTest
    {
        Mock<IBlockedUserDal> _mockBlockedUserDal;
        //Mock<UserManager<ApplicationUser>> _mockUserManager;


        public UserManagementTest()
        {
            var blockedUserList = new List<BlockedUser>
            {
                new BlockedUser {Id=1,BlockingUserName="Test1",BlockedUserName="Test123",BlockedAt=DateTime.Now},
            };

            //var applicationUserList = new List<ApplicationUser>
            //{
            //    new ApplicationUser {UserName="Test1"},
            //}

            //var applicationUser = new ApplicationUser
            //{
            //    UserName="Test1"
            //};
            var mockBlockedUserDal = new Mock<IBlockedUserDal>();
            //var mockUserManager = new Mock<UserManager<ApplicationUser>>();

            mockBlockedUserDal.Setup(mr => mr.GetAll(null)).Returns(blockedUserList);

            mockBlockedUserDal.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int i) => blockedUserList.Single(x => x.Id == i));

            mockBlockedUserDal.Setup(x => x.GetBlockedUserByName(It.IsAny<string>(), It.IsAny<string>())).Returns(blockedUserList);

            mockBlockedUserDal.Setup(mr => mr.Create(It.IsAny<BlockedUser>())).Callback(
               (BlockedUser target) =>
               {
                   blockedUserList.Add(target);
               });

            mockBlockedUserDal.Setup(mr => mr.Update(It.IsAny<BlockedUser>())).Callback(
                (BlockedUser target) =>
                {
                    var original = blockedUserList.Where(q => q.Id == target.Id).Single();

                    if (original == null)
                    {
                        throw new InvalidOperationException();
                    }

                    original.BlockingUserName = target.BlockingUserName;
                    original.BlockedUserName = target.BlockedUserName;

                });
            //mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);


            _mockBlockedUserDal = mockBlockedUserDal;
        }
        //private IBlockedUserService _blockedUserService;

        //[SetUp]
        //public void Setup(IBlockedUserService blockedUserService)
        //{
        //}

        [Test]
        public void BlockedUserService_BlockUser_IsBlockedBefore_ReturnsTrue()
        {
            bool isBlockedBefore = _mockBlockedUserDal.Object.GetBlockedUserByName("Test1", "Test123").Any();

            Assert.That(isBlockedBefore, Is.True);
        }

        [Test]
        public void BlockedUserService_BlockUser_AddBlockedUser()
        {
            var blockedUser = new BlockedUser
            {
                Id=2,
                BlockedAt = DateTime.Now,
                BlockedUserName = "Test5",
                BlockingUserName = "Test1",
            };
            _mockBlockedUserDal.Object.Create(blockedUser);

            var expected = _mockBlockedUserDal.Object.GetById(2);

            Assert.AreEqual(expected.Id, 2);
            Assert.AreEqual(expected.BlockedUserName, "Test5");
            Assert.AreEqual(expected.BlockingUserName, "Test1");
            Assert.IsNotNull(expected);
        }

        [Test]
        public void BlockedUserService_GetAll_ReturnsCount()
        {
            var expected = _mockBlockedUserDal.Object.GetAll().Count;

            Assert.IsNotNull(expected);
            Assert.IsTrue(expected > 0);
        }

        [Test]
        public void BlockedUserService_Update_ReturnsUpdatedValues()
        {
            var actual = new BlockedUser { Id = 1, BlockingUserName = "Test1_Updated", BlockedUserName = "Test123_Updated", BlockedAt = DateTime.Now };

            _mockBlockedUserDal.Object.Update(actual);

            var expected = _mockBlockedUserDal.Object.GetById(actual.Id);

            Assert.IsNotNull(expected);
            Assert.AreEqual(actual.BlockingUserName, expected.BlockingUserName);
            Assert.AreEqual(actual.BlockedUserName, expected.BlockedUserName);
        }
        //[Test]
        //public void BlockedUserService_BlockUser_GetUserByName_ReturnsUser()
        //{
        //    var user =_mockUserManager.Object.FindByNameAsync("Test1").Result.UserName;

        //    Assert.That(user,Is.Null);
        //}
    }
}
