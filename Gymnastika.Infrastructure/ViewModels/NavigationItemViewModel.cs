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

        public NavigationItemViewModel(NavigationDescriptor descriptor, string regionName)
        {
            _navigationDescriptor = descriptor;
            RegionName = regionName;
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
    }
}
