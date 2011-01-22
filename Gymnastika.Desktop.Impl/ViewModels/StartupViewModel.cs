using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Desktop.Core.UserManagement;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Desktop.Views;
using System.Collections.Specialized;
using Microsoft.Practices.Prism.Events;
using System.Windows;

namespace Gymnastika.Desktop.ViewModels
{
    public class StartupViewModel : NotificationObject
    {
        private IUserService _userService;

        public StartupViewModel(IStartupView view, IUserService userService)
        {
            View = view;
            View.Model = this;
            
            _userService = userService;

            RegisteredUsers = new ObservableCollection<User>(_userService.GetAllUsers());
            RegisteredUsers.CollectionChanged += RegisteredUsers_CollectionChanged;
        }

        private void RegisteredUsers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { }

        public IStartupView View { get; set; }

        private ObservableCollection<User> _registeredUsers;

        public ObservableCollection<User> RegisteredUsers
        {
            get { return _registeredUsers; }
            set
            {
                if (_registeredUsers != value)
                {
                    _registeredUsers = value;
                    RaisePropertyChanged("RegisteredUsers");
                }
            }
        }
        
        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    RaisePropertyChanged("SelectedUser");
                }
            }
        }

        private ICommand _logOnCommand;

        public ICommand LogOnCommand
        {
            get
            {
                if (_logOnCommand == null)
                {
                    _logOnCommand = new DelegateCommand<object>(
                        LogOn, obj => RegisteredUsers.Count > 0 && SelectedUser != null);
                }

                return _logOnCommand;
            }
        }

        private ICommand _createNewUserCommand;

        public ICommand CreateNewUserCommand
        {
            get
            {
                if(_createNewUserCommand == null)
                    _createNewUserCommand = new DelegateCommand<object>(CreateNewUser);

                return _createNewUserCommand;
            }
        }

        private void LogOn(object parameter)
        {
            
        }

        private void CreateNewUser(object parameter)
        {
            
        }
    }
}
