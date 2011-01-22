using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.Desktop.ViewModels;
using Gymnastika.Desktop.Core.UserManagement;
using Gymnastika.Desktop.Core.UserManagement.Tests;
using System.Windows.Input;
using Moq;
using Gymnastika.Desktop.Views;

namespace Gymnastika.Desktop.Tests.ViewModels
{
    [TestFixture]
    public class StartupViewModelTests
    {
        StartupViewModel _startViewModel;

        [SetUp]
        public void Init()
        {
            _startViewModel = new StartupViewModel(
                new MockStartupView(),
                new UserService(new InMemoryUserRepository()));
        }

        [Test]
        public void LogOnCommand_SelectedUserNull_CannotExecute()
        {
            Assert.That(_startViewModel.LogOnCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void LogOnCommand_SelectedUserNotNull_CanNotExecute()
        { 
            _startViewModel.SelectedUser = new User();
            Assert.That(_startViewModel.LogOnCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void LogOnCommand_UserSelected_CanExecute()
        {
            User user = new User();
            _startViewModel.RegisteredUsers.Add(user);
            _startViewModel.SelectedUser = user;

            Assert.That(_startViewModel.LogOnCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void LogOnCommand_UserExists_LogOn()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(us => us.LogOn("Martin", null))
                .Returns(true);

            StartupViewModel svm = new StartupViewModel(new MockStartupView(), mockUserService.Object);

            User user = new User() { UserName = "Martin" };
            svm.RegisteredUsers.Add(user);
            svm.SelectedUser = user;

            svm.LogOnCommand.Execute(null);

        }
    }

    public class MockStartupView : IStartupView
    {
        #region IStartupView Members

        public object Model { get; set; }

        #endregion
    }

}
