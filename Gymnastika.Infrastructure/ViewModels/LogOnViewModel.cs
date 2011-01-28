using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Views;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Common.Services;

namespace Gymnastika.ViewModels
{
    public class LogOnViewModel : NotificationObject
    {
        private IUserService _userService;

        public ILogOnView View { get; set; }

        public LogOnViewModel(ILogOnView logOnView, IUserService userService)
        {
            View = logOnView;
            _userService = userService;
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

        public void DoLogOn()
        {
            View.Show();
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

        public event LogOnCompleteHandler LogOnComplete;

        private void OnLogOnComplete(bool result)
        {
            if (LogOnComplete != null)
                LogOnComplete(this, new LogOnCompleteEventArgs(result));
        }

        private void DoProcessLogOn()
        {
            bool result = _userService.LogOn(UserName, Password);
            OnLogOnComplete(result);
        }
    }

    public delegate void LogOnCompleteHandler(object sender, LogOnCompleteEventArgs e);

    public class LogOnCompleteEventArgs : EventArgs
    {
        public LogOnCompleteEventArgs(bool result)
        {
            IsSucceed = result;
        }

        public bool IsSucceed { get; set; }
    }
}
