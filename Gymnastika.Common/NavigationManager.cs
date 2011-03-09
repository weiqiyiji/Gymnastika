using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;

namespace Gymnastika.Common
{
    public class NavigationManager : ObservableCollection<NavigationDescriptor>, INavigationManager
    {
        private readonly IRegionManager _regionManager;

        public NavigationManager(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region INavigationManager Members

        void INavigationManager.AddIfMissing(NavigationDescriptor descriptor, bool isCurrentPage)
        {
            if (this.FirstOrDefault(x => x.ViewType == descriptor.ViewType) == null)
            {
                base.Add(descriptor);
                _regionManager.RegisterViewWithRegion(descriptor.RegionName, descriptor.ViewType);

                if (isCurrentPage)
                    CurrentPage = descriptor;
            }
        }

        void INavigationManager.Remove(NavigationDescriptor descriptor)
        {
            base.Remove(descriptor);
        }

        #endregion

        public event EventHandler CurrentPageChanged;

        public void OnCurrentPageChanged()
        {
            _regionManager.RequestNavigate(CurrentPage.RegionName, CurrentPage.ViewType.Name);
            if (CurrentPageChanged != null)
                CurrentPageChanged(this, EventArgs.Empty);
        }

        private NavigationDescriptor _currentPage;

        public NavigationDescriptor CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if(value == null) throw new ArgumentNullException("CurrentPage");

                if(_currentPage != value)
                {
                    _currentPage = value;
                    OnCurrentPageChanged();
                }
            }
        }
    }
}
