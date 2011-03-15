using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Gymnastika.Common;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Common.Navigation;
using System.Collections.ObjectModel;

namespace Gymnastika.ViewModels
{
    public class NavigationViewModel : NotificationObject
    {
        private INavigationManager _navigationManager;
        private ObservableCollection<NavigationRegionViewModel> _regions;

        public NavigationViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _regions = new ObservableCollection<NavigationRegionViewModel>(
                _navigationManager.Regions.Select(r => new NavigationRegionViewModel(r)));

            _navigationManager.Regions.CollectionChanged += OnRegionsChanged;
        }

        private void OnRegionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INavigationRegion region in e.NewItems)
                {
                    _regions.Add(new NavigationRegionViewModel(region));
                }
            }
        }

        public ObservableCollection<NavigationRegionViewModel> Regions 
        {
            get { return _regions; }
        }
    }
}
