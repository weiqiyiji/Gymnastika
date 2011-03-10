using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Gymnastika.Common;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.ViewModels
{
    public class NavigationViewModel : NotificationObject
    {
        private INavigationManager _navigationManager;

        public NavigationViewModel(INavigationManager navigationManager)
        {
            Targets = navigationManager;
        }

        public INavigationManager Targets
        {
            get { return _navigationManager; }
            set
            {
                if (_navigationManager != value)
                {
                    _navigationManager = value;
                    RaisePropertyChanged("Targets");
                }
            }
        }

        public NavigationDescriptor CurrentPage
        {
            get { return Targets.CurrentPage; }
            set
            {
                if (Targets.CurrentPage != value)
                {
                    Targets.CurrentPage = value;
                    RaisePropertyChanged("CurrentPage");
                }
            }
        }	
    }
}
