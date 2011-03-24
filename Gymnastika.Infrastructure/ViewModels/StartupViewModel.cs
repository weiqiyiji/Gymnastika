using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Controllers;
using Gymnastika.Data;
using Gymnastika.Services.Contracts;
using Gymnastika.Services.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;

namespace Gymnastika.ViewModels
{
    public class StartupViewModel : NotificationObject
    {
        private IUserService _userService;
        private IUnityContainer _container;

        public StartupViewModel(IUnityContainer container, IUserService userService)
        {
            _container = container;
            _userService = userService;

            using(container.Resolve<IWorkEnvironment>().GetWorkContextScope())
            {
                RegisteredUsers = new ObservableCollection<User>(_userService.GetAllUsers());
            }
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
            get
            {
                if(_selectedUser == null)
                {
                    if (_registeredUsers != null && _registeredUsers.Count > 0)
                        return _registeredUsers[0];
                }
                return _selectedUser;
            }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    RaisePropertyChanged("SelectedUser");
                    //(LogOnCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
                //else
                //{
                //    if (_selectedUser != null)
                //    {
                //        _container.Resolve<IStartupController>().RequestLogOn(SelectedUser.UserName);
                //    }
                //}
            }
        }

        private ICommand _logOnCommand;

        public ICommand LogOnCommand
        {
            get
            {
                if (_logOnCommand == null)
                {
                    _logOnCommand = new DelegateCommand(LogOn, CommandCanExecutePredicate);
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
                    _createNewUserCommand = new DelegateCommand(CreateNewUser);

                return _createNewUserCommand;
            }
        }
        
        private ICommand _goToPreviousCommand;

        public ICommand GoToPreviousCommand
        {
            get
            {
                if (_goToPreviousCommand == null)
                    _goToPreviousCommand = new DelegateCommand(GoToPrevious, CommandCanExecutePredicate);

                return _goToPreviousCommand;
            }
        }
        
        private ICommand _goToNextCommand;

        public ICommand GoToNextCommand
        {
            get
            {
                if (_goToNextCommand == null)
                    _goToNextCommand = new DelegateCommand(GoToNext, CommandCanExecutePredicate);

                return _goToNextCommand;
            }
        }
					
        private void GoToPrevious()
        {
            int index = RegisteredUsers.IndexOf(SelectedUser);
            if (index == 0)
                index = RegisteredUsers.Count - 1;
            else
                index--;

            SelectedUser = RegisteredUsers[index];
        }

        private void GoToNext()
        {
            int index = RegisteredUsers.IndexOf(SelectedUser);
            if (index == (RegisteredUsers.Count - 1))
                index = 0;
            else
                index++;

            SelectedUser = RegisteredUsers[index];
        }

        private void LogOn()
        {
            _container.Resolve<IStartupController>().RequestLogOn(SelectedUser.UserName);
        }

        private bool CommandCanExecutePredicate()
        {
            return RegisteredUsers.Count > 0 && SelectedUser != null;
        }

        private void CreateNewUser()
        {
            _container.Resolve<IStartupController>().RequestCreateNewUser();
        }
    }
}
