using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.Desktop.ViewModels;
using Gymnastika.UserManagement;
using Gymnastika.UserManagement.Tests;
using System.Windows.Input;

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
                new UserService(new InMemoryUserRepository()));
        }

        [Test]
        public void LogOnCommand_SelectedUserNull_CannotExecute()
        {
            Assert.That(_startViewModel.LogOnCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void LogOnCommand_SelectedUserNotNull_CanExecute()
        { 
            _startViewModel.SelectedUser = new User();
            Assert.That(_startViewModel.LogOnCommand.CanExecute(null), Is.False);
        }
    }
}
