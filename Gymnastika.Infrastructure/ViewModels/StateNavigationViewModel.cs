using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Navigation;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.ViewModels
{
    public class StateNavigationViewModel : NotificationObject
    {
        private ViewState _viewState;
        private NavigationDescriptor _descriptor;
        private string _regionName;
        private INavigationService _navigationService;

        public StateNavigationViewModel(ViewState viewState, NavigationDescriptor descriptor, string regionName)
        {
            _viewState = viewState;
            _descriptor = descriptor;
            _regionName = regionName;
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _navigationService.NavigationStart += OnNavigationStart;
        }
        
        private string _header;

        public string Header
        {
            get { return _viewState.Header; }
        }
        
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }
				
        
        private ICommand _requestNavigateCommand;

        public ICommand RequestNavigateCommand
        {
            get
            {
                if (_requestNavigateCommand == null)
                    _requestNavigateCommand = new DelegateCommand(RequestNavigate);

                return _requestNavigateCommand;
            }
        }

        private void RequestNavigate()
        {
            INavigationService navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.RequestNavigate(_regionName, _descriptor.ViewName, _viewState.Name);
        }

        private void OnNavigationStart(object sender, NavigationEventArgs e)
        {
            IsSelected = (e.TargetDescriptor == _descriptor && e.TargetState == _viewState);
        }
    }
}
