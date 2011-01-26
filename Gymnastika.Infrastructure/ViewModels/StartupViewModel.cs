using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Gymnastika.Common.Events;
using Gymnastika.Common.UserManagement;
using Gymnastika.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Gymnastika.ViewModels
{
    public class StartupViewModel : NotificationObject
    {
        private IUserService _userService;
        private IUnityContainer _container;

        public StartupViewModel(IUnityContainer container, IStartupView view, IUserService userService)
        {
            View = view;
            View.Model = this;

            _container = container;
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
            LogOnViewModel vm = _container.Resolve<LogOnViewModel>();
            vm.UserName = SelectedUser.UserName;
            vm.LogOnComplete += LogOnViewModel_LogOnComplete;
            vm.DoLogOn();
        }

        private void LogOnViewModel_LogOnComplete(object sender, LogOnCompleteEventArgs e)
        {
            if (e.IsSucceed)
            {
                ServiceLocator.Current.GetInstance<IEventAggregator>()
                    .GetEvent<LogOnSuccessEvent>().Publish(SelectedUser);
            }
        }

        private void CreateNewUser(object parameter)
        {
            
        }
    }
}
