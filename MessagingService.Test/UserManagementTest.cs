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

        public UserManagementTest()
        {
            var blockedUserList = new List<BlockedUser>
            {
                new BlockedUser {Id=1,BlockingUserName="Test1",BlockedUserName="Test123",BlockedAt=DateTime.Now},
            };

            var mockBlockedUserDal = new Mock<IBlockedUserDal>();

            mockBlockedUserDal.Setup(mr => mr.GetAll(null)).Returns(blockedUserList);

            mockBlockedUserDal.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int i) => blockedUserList.SingleOrDefault(x => x.Id == i));

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

            _mockBlockedUserDal = mockBlockedUserDal;
        }
       
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
        [Test]
        public void BlockedUserService_GetBlockedUserById_ShouldNotReturnNull()
        {
            var expected = _mockBlockedUserDal.Object.GetById(1);

            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.BlockingUserName, "Test1");
            Assert.AreEqual(expected.BlockedUserName, "Test123");
        }

        [TestCase(3, ExpectedResult = null)]
        [TestCase(4, ExpectedResult = null)]
        [TestCase(5, ExpectedResult = null)]
        [Test]
        public BlockedUser BlockedUserService_GetBlockedUserByWrongId_ShouldReturnNull(int id)
        {
            return _mockBlockedUserDal.Object.GetById(id);
        }
    }
}
