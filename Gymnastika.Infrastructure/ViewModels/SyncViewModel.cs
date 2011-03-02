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
        private readonly ConnectionStore _connectionStore;

        public SyncViewModel(RegistrationService registrationService, ConnectionStore connectionStore)
        {
            _registrationService = registrationService;
            _connectionStore = connectionStore;
            Status = "未连接";
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
            if (_connectionStore.IsRegistered)
            {
                var response = _registrationService.Connect(_connectionStore.AssignedId.ToString(), PhoneId);
                if (response.HasError)
                {
                    Status = response.ErrorMessage;
                }

                _connectionStore.SaveConnection(
                    int.Parse(
                        StringHelper.GetPureString(response.Response.Content.ReadAsString())));

                Status = "连接成功";
            }
            else
            {
                Status = "还未注册";
            }
        }
    }
}
