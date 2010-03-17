using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Common.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.ViewModels
{
    public class NavigationItemViewModel : NotificationObject
    {
        private NavigationDescriptor _navigationDescriptor;
        private INavigationService _navigationService;

        public NavigationItemViewModel(NavigationDescriptor descriptor, string regionName)
        {
            _navigationDescriptor = descriptor;
            RegionName = regionName;
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _navigationService.NavigationStart += OnNavigationStart;
        }

        public string Header 
        {
            get { return _navigationDescriptor.Header; }
        }

        public string RegionName { get; set; }
        
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
            INavigationService service = ServiceLocator.Current.GetInstance<INavigationService>();
            service.RequestNavigate(RegionName, _navigationDescriptor.ViewName);
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

        private void OnNavigationStart(object sender, NavigationEventArgs e)
        {
            IsSelected = (e.TargetDescriptor == _navigationDescriptor && e.TargetState == null);
        }
    }
}
