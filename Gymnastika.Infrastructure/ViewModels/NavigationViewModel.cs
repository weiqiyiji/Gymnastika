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
            CurrentPage = Targets.CurrentPage;
            Targets.CurrentPageChanged += new EventHandler(Targets_CurrentPageChanged);
        }

        private void Targets_CurrentPageChanged(object sender, EventArgs e)
        {
            CurrentPage = Targets.CurrentPage;
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

        private NavigationDescriptor _currentPage;

        public NavigationDescriptor CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    Targets.CurrentPage = _currentPage;
                    RaisePropertyChanged("CurrentPage");
                }
            }
        }	
    }
}
