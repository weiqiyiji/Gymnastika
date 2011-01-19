using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.UserManagement;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Desktop.ViewModels
{
    public class StartupViewModel : NotificationObject
    {
        private IUserService _userService;

        public StartupViewModel(IUserService userService)
        {
            _userService = userService;
            RegisteredUsers = new ObservableCollection<User>(_userService.GetAllUsers());
            RegisteredUsers.CollectionChanged += RegisteredUsers_CollectionChanged;
        }

        private void RegisteredUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("RegisteredUsers");
        }

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
    }
}
