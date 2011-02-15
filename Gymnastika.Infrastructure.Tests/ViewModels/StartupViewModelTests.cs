using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.ViewModels;
using Gymnastika.Services;
using System.Windows.Input;
using Moq;
using Gymnastika.Views;
using Microsoft.Practices.Unity;
using Gymnastika.Services.Models;
using Gymnastika.Tests.Support;
using Gymnastika.Services.Impl;

namespace Gymnastika.Tests.ViewModels
{
    [TestFixture]
    public class StartupViewModelTests
    {
        StartupViewModel _startViewModel;

        [SetUp]
        public void Init()
        {
            _startViewModel = new StartupViewModel(
                new UnityContainer(),
                new UserService(new InMemoryRepository<User>()));
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
    }

    public class MockStartupView : IStartupView
    {
        #region IStartupView Members

        public object Model { get; set; }

        #endregion

        #region IStartupView Members


        public void DisplayLogOnField()
        {
            
        }

        #endregion
    }

}
