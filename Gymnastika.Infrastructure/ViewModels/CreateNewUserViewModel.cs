using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;
using Gymnastika.Services.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Services;
using Gymnastika.Services.Session;
using Microsoft.Practices.Prism.Events;

using Gymnastika.Services.Contracts;
using Gymnastika.Events;

namespace Gymnastika.ViewModels
{
    public class CreateNewUserViewModel : NotificationObject
    {
        private IUserService _userService;
        private ISessionManager _sessionManager;
        private IEventAggregator _eventAggregator;
        private User _user;

        public CreateNewUserViewModel(
            IUserService userService, ISessionManager sessionManager, IEventAggregator eventAggregator)
        {
            _userService = userService;
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;
            _user = new User();
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
            get { return _user.Age.ToString(); }
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
            get { return _user.Height.ToString(); }
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
            get { return _user.Weight.ToString(); }
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
        
        private ICommand _submitCommand;

        public ICommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                    _submitCommand = new DelegateCommand(Submit);

                return _submitCommand;
            }
        }

        private void Submit()
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
    }
}
