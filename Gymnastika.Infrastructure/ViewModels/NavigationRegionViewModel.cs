using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Navigation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections;

namespace Gymnastika.ViewModels
{
    public class NavigationRegionViewModel : NotificationObject
    {
        private INavigationRegion _navigationRegion;
        private ObservableCollection<object> _viewItems;

        public NavigationRegionViewModel(INavigationRegion navigationRegion)
        {
            _navigationRegion = navigationRegion;
            _viewItems = new ObservableCollection<object>();
            PopulateRegion(navigationRegion.NavigationViews);

            _navigationRegion.NavigationViews.CollectionChanged += OnNavigationViewsChanged;
        }

        private void OnNavigationViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PopulateRegion(e.NewItems);
            }
        }

        private void PopulateRegion(IEnumerable descriptors)
        {
            foreach (NavigationDescriptor descriptor in descriptors)
            {
                _viewItems.Add(new NavigationItemViewModel(descriptor, _navigationRegion.Name));
                if (descriptor.States != null && descriptor.States.Count > 0)
                {
                    foreach (var viewState in descriptor.States)
                    {
                        _viewItems.Add(
                            new StateNavigationViewModel(viewState, descriptor, _navigationRegion.Name));
                    }
                }
            }
        }

        public string Header { get { return _navigationRegion.Header; } }

        public IEnumerable ViewItems { get { return _viewItems; } }
    }
}
