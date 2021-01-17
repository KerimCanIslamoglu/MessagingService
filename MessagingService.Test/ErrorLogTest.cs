using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using MessagingService.Entities.Entities;
using NUnit.Framework;
using MessagingService.DataAccess.Abstract;
using System.Linq;

namespace MessagingService.Test
{
    [TestFixture]
    public class ErrorLogTest
    {
        Mock<IErrorLogsDal> _mockErrorLogsDal;

        public ErrorLogTest()
        {
            var errorLogsList = new List<ErrorLogs>
            {
                new ErrorLogs {Id=1,CreatedAt=DateTime.Now,ErrorDetail="Test1",MethodName="TestMethod1",UserName="Test1"},
            };
           
            var mockErrorLogsDal = new Mock<IErrorLogsDal>();

            mockErrorLogsDal.Setup(mr => mr.GetAll(null)).Returns(errorLogsList);

            mockErrorLogsDal.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int i) => errorLogsList.Single(x => x.Id == i));

            mockErrorLogsDal.Setup(mr => mr.Create(It.IsAny<ErrorLogs>())).Callback(
               (ErrorLogs target) =>
               {
                   errorLogsList.Add(target);
               });

            mockErrorLogsDal.Setup(mr => mr.Update(It.IsAny<ErrorLogs>())).Callback(
                (ErrorLogs target) =>
                {
                    var original = errorLogsList.Where(q => q.Id == target.Id).Single();

                    if (original == null)
                    {
                        throw new InvalidOperationException();
                    }

                    original.ErrorDetail = target.ErrorDetail;
                    original.MethodName = target.MethodName;
                    original.UserName = target.UserName;

                });


            _mockErrorLogsDal = mockErrorLogsDal;
        }
       
        [Test]
        public void ErrorLog_Insert_AddingErrorLog()
        {
            var errorLogs = new ErrorLogs
            {
                Id = 2,
               CreatedAt=DateTime.Now,
               ErrorDetail= "Test2",
               MethodName= "TestMethod2",
               UserName="Test2",
            };
            _mockErrorLogsDal.Object.Create(errorLogs);

            var expected = _mockErrorLogsDal.Object.GetById(2);

            Assert.AreEqual(expected.Id, 2);
            Assert.AreEqual(expected.ErrorDetail, "Test2");
            Assert.AreEqual(expected.MethodName, "TestMethod2");
            Assert.AreEqual(expected.UserName, "Test2");
            Assert.IsNotNull(expected);
        }

        [Test]
        public void ErrorLog_GetAll_ReturnsCount()
        {
            var expected = _mockErrorLogsDal.Object.GetAll().Count;

            Assert.IsNotNull(expected);
            Assert.IsTrue(expected > 0);
        }

        [Test]
        public void ErrorLog_Update_ReturnsUpdatedValues()
        {
            var actual = new ErrorLogs { Id = 1, ErrorDetail = "Test2_Updated", MethodName = "TestMethod2_Updated", UserName= "Test2_Updated", CreatedAt = DateTime.Now };

            _mockErrorLogsDal.Object.Update(actual);

            var expected = _mockErrorLogsDal.Object.GetById(actual.Id);

            Assert.IsNotNull(expected);
            Assert.AreEqual(actual.ErrorDetail, expected.ErrorDetail);
            Assert.AreEqual(actual.MethodName, expected.MethodName);
            Assert.AreEqual(actual.UserName, expected.UserName);
        }
    }
}
