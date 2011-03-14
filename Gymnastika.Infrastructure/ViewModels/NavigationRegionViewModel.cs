using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Navigation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.ViewModels
{
    public class NavigationRegionViewModel : NotificationObject
    {
        private INavigationRegion _navigationRegion;
        private ObservableCollection<NavigationItemViewModel> _viewItems;

        public NavigationRegionViewModel(INavigationRegion navigationRegion)
        {
            _viewItems = new ObservableCollection<NavigationItemViewModel>(
                navigationRegion.NavigationViews.Select(
                    x => new NavigationItemViewModel(x, navigationRegion.Name)));

            _navigationRegion = navigationRegion;
            _navigationRegion.NavigationViews.CollectionChanged += OnNavigationViewsChanged;
        }

        private void OnNavigationViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NavigationDescriptor descriptor in e.NewItems)
                {
                    _viewItems.Add(new NavigationItemViewModel(descriptor, _navigationRegion.Name));
                }
            }
        }

        public string Header { get { return _navigationRegion.Header; } }

        public IEnumerable<NavigationItemViewModel> ViewItems { get { return _viewItems; } }
    }
}
