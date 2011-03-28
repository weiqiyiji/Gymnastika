using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Gymnastika.Data;
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
        private readonly IWorkEnvironment _workEnvironment;
        private readonly IUserService _userService;
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;
        private User _user;

        public const int LogOnTabIndex = 0;
        public const int CreateNewUserTabIndex = 1;
        
        public UserProfileViewModel(
            IWorkEnvironment workEnvironment,
            IUserService userService, 
            ISessionManager sessionManager, 
            IEventAggregator eventAggregator)
        {
            _workEnvironment = workEnvironment;
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
        
        private bool _hasError;

        public bool HasError
        {
            get { return _hasError; }
            set
            {
                if (_hasError != value)
                {
                    _hasError = value;
                    RaisePropertyChanged("HasError");
                }
            }
        }
							
        private ICommand _backCommand;

        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new DelegateCommand(() => GoBack(null));

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

        private bool _lockLogOn = false;

        private void ProcessLogOn()
        {
            if (_lockLogOn) return;

            _lockLogOn = true;

            User savedUser = null;
            bool isLogOnOk = false;

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                if ((isLogOnOk = _userService.LogOn(UserName, Password)))
                {
                    savedUser = _userService.GetUser(UserName);
                }
            }

            if (isLogOnOk)
            {
                GoBack(savedUser);
            }
            else
            {
                HasError = true;
                ErrorMessage = _userService.ErrorString;
            }

            _lockLogOn = false;
        }

        private bool ValidateCreateUserForm()
        {
            //TODO
            return true;
        }

        private void CreateNewUser()
        {
            User savedUser = null;

            IsRegisterFailed = false;
            ErrorMessage = string.Empty;
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                savedUser = _userService.GetUser(UserName);

                if (savedUser == null)
                {
                    savedUser = _userService.Register(_user);
                    _sessionManager.Add(savedUser);
                }
                else
                {
                    IsRegisterFailed = true;
                    ErrorMessage = _userService.ErrorString;
                }
            }
            if (!IsRegisterFailed)
            {
                GoBack(savedUser);
            }
        }	
		
	    private void GoBack(User user)
        {
            HasError = false;
            ErrorMessage = null;
            NotifyClose = true;
            _eventAggregator.GetEvent<LogOnCompleteEvent>().Publish(user);
	    }
    }
}
