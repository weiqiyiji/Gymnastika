using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.ViewModels;
using Gymnastika.Common.Services;
using Gymnastika.Common.Models;
using Gymnastika.Common.Session;
using Moq;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Common.Events;

namespace Gymnastika.Infrastructure.Tests.ViewModels
{
    [TestFixture]
    public class CreateNewUserViewModelTests
    {
        [Test]
        public void SubmitCommand_EventRegistered()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            bool eventRegistered = false;
            UserModel um = new UserModel();

            eventAggregator.GetEvent<LogOnSuccessEvent>().Subscribe(u => um.Id = u.Id);

            ISessionManager sessionManager = new SessionManager();
            CreateNewUserViewModel vm = new CreateNewUserViewModel(
                new MockUserService(), sessionManager, eventAggregator);

            vm.SubmitCommand.Execute(null);

            Assert.That(um.Id, Is.EqualTo(1));
        }

        [Test]
        public void SubmitCommand()
        {
            var userService = new MockUserService();
            ISessionManager sessionManager = new SessionManager();
            CreateNewUserViewModel vm = new CreateNewUserViewModel(
                userService, sessionManager, new EventAggregator());

            Assert.That(vm.IsRegisterFailed, Is.False);

            vm.SubmitCommand.Execute(null);

            Assert.That(vm.IsRegisterFailed, Is.False);
            
            SessionContext context = sessionManager.GetCurrentSession();
            
            Assert.That(context, Is.Not.Null);
            Assert.That(context.AssociatedUser.Id, Is.EqualTo(userService.RegisteredUser.Id));
        }

        [Test]
        public void SubmitCommand_DuplicateUserName_RegisterFailed()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(us => us.GetUser(It.IsAny<string>()))
                .Returns(new UserModel());

            ISessionManager sessionManager = new SessionManager();
            CreateNewUserViewModel vm = new CreateNewUserViewModel(
                mockUserService.Object, sessionManager, new Mock<IEventAggregator>().Object);

            Assert.That(vm.IsRegisterFailed, Is.False);

            vm.SubmitCommand.Execute(null);

            Assert.That(vm.IsRegisterFailed, Is.True);
        }
    }

    internal class MockUserService : IUserService
    {
        public UserModel RegisteredUser { get; set; }

        #region IUserService Members

        public string ErrorString
        {
            get { return string.Empty; }
        }

        public UserModel GetUser(int id)
        {
            return null;
        }

        public UserModel GetUser(string userName)
        {
            return null;
        }

        public UserModel GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public bool LogOn(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool LogOut(string userName)
        {
            throw new NotImplementedException();
        }

        public UserModel Register(UserModel user)
        {
            RegisteredUser = user;
            RegisteredUser.Id = 1;
            return RegisteredUser;
        }

        public void Update(UserModel u)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
