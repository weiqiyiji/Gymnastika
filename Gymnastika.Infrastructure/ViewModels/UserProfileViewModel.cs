using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Gymnastika.Events;
using Gymnastika.Services.Contracts;
using Gymnastika.Services.Models;
using Gymnastika.Services.Session;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.ViewModels
{
    public class UserProfileViewModel : NotificationObject
    {
        private readonly IUserService _userService;
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;
        private User _user;
        public const int LogOnTabIndex = 0;
        public const int CreateNewUserTabIndex = 0;
        
        public UserProfileViewModel(
            IUserService userService, ISessionManager sessionManager, IEventAggregator eventAggregator)
        {
            _userService = userService;
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;
            _user = new User();
        }

        private int _initialTabIndex;

        public int InitialTabIndex
        {
            get { return _initialTabIndex; }
            set
            {
                if (_initialTabIndex != value)
                {
                    _initialTabIndex = value;
                    RaisePropertyChanged("InitialTabIndex");
                }
            }
        }

        public string UserName
        {
            get { return _user.UserName; }
            set
            {
                if (_user.UserName != value)
                {
                    _user.UserName = value;
                    RaisePropertyChanged("UserName");
                    (LogOnCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }
        
        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    RaisePropertyChanged("ConfirmPassword");
                }
            }
        }		

        public string Password
        {
            get { return _user.Password; }
            set
            {
                if (_user.Password != value)
                {
                    _user.Password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        public string Age
        {
            get
            {
                return _user.Age == 0 ? string.Empty : _user.Age.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                int age = int.Parse(value);
                if (_user.Age != age)
                {
                    _user.Age = age;
                    RaisePropertyChanged("Age");
                }
            }
        }

        public string Height
        {
            get
            {
                return _user.Height == 0 ? string.Empty : _user.Height.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                int height = int.Parse(value);
                if (_user.Height != height)
                {
                    _user.Height = height;
                    RaisePropertyChanged("Height");
                }
            }
        }

        public string Weight
        {
            get
            {
                return _user.Weight == 0 ? string.Empty : _user.Weight.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                int weight = int.Parse(value);
                if (_user.Weight != weight)
                {
                    _user.Weight = weight;
                    RaisePropertyChanged("Weight");
                }
            }
        }

        public Gender Gender
        {
            get { return _user.Gender; }
            set
            {
                if (_user.Gender != value)
                {
                    _user.Gender = value;
                    RaisePropertyChanged("Gender");
                }
            }
        }

        public int GenderIndex
        {
            get { return (int) Gender; }
            set
            {
                if((int)Gender != value)
                {
                    Gender = (Gender)value;
                    RaisePropertyChanged("GenderIndex");
                }
            }
        }

        private bool _isRegisterFailed;

        public bool IsRegisterFailed
        {
            get { return _isRegisterFailed; }
            set
            {
                if (_isRegisterFailed != value)
                {
                    _isRegisterFailed = value;
                    RaisePropertyChanged("IsRegisterFailed");
                }
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged("ErrorMessage");
                }
            }
        }
        
        private bool _notifyClose;

        public bool NotifyClose
        {
            get { return _notifyClose; }
            set
            {
                if (_notifyClose != value)
                {
                    _notifyClose = value;
                    RaisePropertyChanged("NotifyClose");
                }
            }
        }
				
        private ICommand _backCommand;

        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new DelegateCommand(GoBack);

                return _backCommand;
            }
        }
        
        private ICommand _createNewUserCommand;

        public ICommand CreateNewUserCommand
        {
            get
            {
                if (_createNewUserCommand == null)
                    _createNewUserCommand = new DelegateCommand(CreateNewUser, ValidateCreateUserForm);

                return _createNewUserCommand;
            }
        }

        private ICommand _logOnCommand;

        public ICommand LogOnCommand
        {
            get
            {
                if (_logOnCommand == null)
                    _logOnCommand = new DelegateCommand(ProcessLogOn, () => !string.IsNullOrEmpty(UserName));

                return _logOnCommand;
            }
        }

        private void ProcessLogOn()
        {
            if (_userService.LogOn(UserName, Password))
            {
                _eventAggregator.GetEvent<LogOnSuccessEvent>().Publish(_userService.GetUser(UserName));
            }
        }

        private bool ValidateCreateUserForm()
        {
            //TODO
            return true;
        }

        private void CreateNewUser()
        {
            IsRegisterFailed = false;
            ErrorMessage = string.Empty;

            User registeredUser = _userService.GetUser(UserName);
            if (registeredUser == null)
            {
                User savedUser = _userService.Register(_user);
                _sessionManager.Add(savedUser);
                _eventAggregator.GetEvent<LogOnSuccessEvent>().Publish(savedUser);
            }
            else
            {
                IsRegisterFailed = true;
                ErrorMessage = _userService.ErrorString;
            }
        }	
		
	    private void GoBack()
	    {
	        NotifyClose = true;
	    }
    }
}
