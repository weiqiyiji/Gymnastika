using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Sync.Communication.Client;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.ViewModels
{
    public class SyncViewModel : NotificationObject
    {
        private readonly RegistrationService _registrationService;

        public SyncViewModel(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        private string _phoneId;

        public string PhoneId
        {
            get { return _phoneId; }
            set
            {
                if (_phoneId != value)
                {
                    _phoneId = value;
                    RaisePropertyChanged("PhoneId");
                    (ConnectToPhoneCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }
        
        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }
        
        private bool _isConnecting;

        public bool IsConnecting
        {
            get { return _isConnecting; }
            set
            {
                if (_isConnecting != value)
                {
                    _isConnecting = value;
                    RaisePropertyChanged("IsConnecting");
                }
            }
        }
				
        private ICommand _connectToPhoneCommand;

        public ICommand ConnectToPhoneCommand
        {
            get
            {
                if (_connectToPhoneCommand == null)
                    _connectToPhoneCommand = new DelegateCommand(ConnectToPhone, () => !string.IsNullOrEmpty(PhoneId));

                return _connectToPhoneCommand;
            }
        }

        private void ConnectToPhone()
        {
        }
    }
}
