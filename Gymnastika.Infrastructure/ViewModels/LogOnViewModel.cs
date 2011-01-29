using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Views;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Common.Services;
using Gymnastika.Common.Models;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Common.Events;

namespace Gymnastika.ViewModels
{
    public class LogOnViewModel : NotificationObject
    {
        private IUserService _userService;
        private IEventAggregator _eventAggregator;

        public LogOnViewModel(IUserService userService, IEventAggregator eventAggregator)
        {
            _userService = userService;
            _eventAggregator = eventAggregator;
        }
     
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        
        private bool _isSuccess;

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set
            {
                if (_isSuccess != value)
                {
                    _isSuccess = value;
                    RaisePropertyChanged("IsSuccess");
                }
            }
        }
				

        private ICommand _logOnCommand;

        public ICommand LogOnCommand
        {
            get
            {
                if (_logOnCommand == null)
                    _logOnCommand = new DelegateCommand(DoProcessLogOn, () => !string.IsNullOrEmpty(UserName));

                return _logOnCommand;
            }
        }

        private void DoProcessLogOn()
        {
            IsSuccess = _userService.LogOn(UserName, Password);

            if (IsSuccess)
            {
                _eventAggregator.GetEvent<LogOnSuccessEvent>().Publish(_userService.GetUser(UserName));
            }
        }
    }
}
