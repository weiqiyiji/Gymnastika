using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.ProjectResources.Properties;
using Moq;
using Gymnastika.Common.Services;
using Gymnastika.Common.Repositories;
using Gymnastika.Common.Models;
using Gymnastika.Tests.Support;

namespace Gymnastika.Common.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;

        [SetUp]
        public void Init()
        {
            _userService = new UserService(new InMemoryUserRepository());
        }

        [Test]
        public void Register_InfoValid_ReturnUserWithId()
        {
            UserModel user = _userService.Register(
                new UserModel 
                { 
                    UserName = "Martin", 
                    Password = "Pwd",
                    Gender = Gender.Male,
                    Age = 21
                });

            Assert.IsNotNull(user);
            Assert.AreNotEqual(default(Guid), user.Id);
            Assert.AreEqual("Martin", user.UserName);
            Assert.AreEqual(Gender.Male, user.Gender);
            Assert.AreEqual(21, user.Age);
            Assert.AreEqual("Pwd", user.Password);
        }

        [Test]
        public void Register_PasswordNull_CanPass()
        {
            UserModel user = _userService.Register(
                new UserModel
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = 21
                });

            Assert.IsNotNull(user);
            Assert.IsNull(_userService.ErrorString);
            Assert.AreNotEqual(default(Guid), user.Id);
            Assert.AreEqual("Martin", user.UserName);
            Assert.AreEqual(Gender.Male, user.Gender);
            Assert.AreEqual(21, user.Age);
        }

        [Test]
        public void Register_UserExists_ReturnNull()
        {
            _userService.Register(
                new UserModel
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = 21
                });

            UserModel user = _userService.Register(
                new UserModel
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = 21
                });

            Assert.IsNull(user);
            Assert.AreEqual(Resources.DuplicateUserName, _userService.ErrorString);
        }

        [Test]
        public void Register_AgeNotValid_ReturnNull()
        {
            UserModel user = _userService.Register(
                new UserModel
                {
                    UserName = "Martin",
                    Gender = Gender.Male,
                    Age = -1
                });

            Assert.IsNull(user);
            Assert.AreEqual(Resources.InvalidAge, _userService.ErrorString);
        }

        [Test]
        public void Register_UserNameNull_ReturnNull()
        {
            UserModel user = _userService.Register(new UserModel());
            Assert.IsNull(user);
            Assert.AreEqual(Resources.InvalidUserName, _userService.ErrorString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_UserNull_Throws()
        {
            _userService.Register(null);
        }

        [Test]
        public void LogOn_InfoValid_ReturnTrue()
        {
            _userService.Register(
                new UserModel { UserName = "Martin", Password = "Pwd", IsActive = false });

            bool result = _userService.LogOn("Martin", "Pwd");

            Assert.IsTrue(result);

            UserModel user = _userService.GetUser("Martin");

            Assert.IsTrue(user.IsActive);
        }

        [Test]
        public void LogOn_UserNamePasswordMissmatch_ReturnFalse()
        {
            var mockRepository = new Mock<IUserRepository>();
            mockRepository
                .Setup(m => m.Get("Martin"))
                .Returns(new UserModel { Password = "Pwd" });

            UserService userService = new UserService(mockRepository.Object);
            bool result = userService.LogOn("Martin", "InvalidPassword");

            Assert.IsFalse(result);
            Assert.AreEqual(Resources.InvalidPassword, userService.ErrorString);

            mockRepository.VerifyAll();
        }

        [Test]
        public void LogOn_UserNameNull_ReturnFalse()
        {
            bool result = _userService.LogOn(string.Empty, null);

            Assert.IsFalse(result);
            Assert.AreEqual(Resources.InvalidUserName, _userService.ErrorString);
        }

        [Test]
        public void LogOut_UserNameExists_ReturnTrue()
        {
            _userService.Register(
                new UserModel { UserName = "Martin", Password = "Pwd", IsActive = true });

            bool result = _userService.LogOut("Martin");

            Assert.IsTrue(result);

            UserModel user = _userService.GetUser("Martin");
            Assert.IsFalse(user.IsActive);
        }

        [Test]
        public void LogOut_UserNameNotExists_ReturnFalse()
        {
            bool result = _userService.LogOut("NotExistsUser");

            Assert.IsFalse(result);
        }

        [Test]
        public void Update()
        {
            _userService.Register(
                new UserModel { UserName = "Martin", Password = "Pwd", IsActive = true, Age = 20 });

            UserModel user = _userService.GetUser("Martin");
            user.Age = 30;
            _userService.Update(user);

            user = _userService.GetUser("Martin");
            Assert.AreEqual(30, user.Age);
        }
    }
}
