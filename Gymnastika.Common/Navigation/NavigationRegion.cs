using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Specialized;
using System.Collections;

namespace Gymnastika.Common.Navigation
{
    public class NavigationRegion : INavigationRegion
    {
        private NavigationViewCollection _navigationViews;
        private NavigationViewCollection _activeNavigationViews;
        private INavigationContainerAccessor _containerAccessor;

        public NavigationRegion(INavigationContainerAccessor containerAccessor)
        {
            _navigationViews = new NavigationViewCollection();
            _activeNavigationViews = new NavigationViewCollection();
            _containerAccessor = containerAccessor;
            if (!_containerAccessor.IsContainerReady)
            {
                _containerAccessor.ContainerReady += OnContainerReady;
            }
            else
            {
                PopulateContainer();
            }
        }

        private void PopulateContainer()
        {
            AddViewsToContainer(this.ActiveNavigationViews);

            ActiveNavigationViews.CollectionChanged += OnNavigationViewsCollectionChanged;
        }

        private void OnContainerReady(object sender, EventArgs e)
        {
            PopulateContainer();
        }

        private void OnNavigationViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddViewsToContainer(e.NewItems);
            }
        }

        private void AddViewsToContainer(IEnumerable views)
        {
            foreach (NavigationDescriptor descriptor in views)
            {
                var view = descriptor.ViewResolver();
                _containerAccessor.Container.Items.Add(view);
            }
        }

        #region INavigationRegion Members

        public string Name { get; set; }

        public string Header { get; set; }

        public INavigationViewCollection NavigationViews 
        {
            get { return _navigationViews; }
        }

        public INavigationViewCollection ActiveNavigationViews
        {
            get { return _activeNavigationViews; }
        }

        public void Add(NavigationDescriptor descriptor)
        {
            _navigationViews.Add(descriptor);

            if (!descriptor.DelayLoad)
            {
                _activeNavigationViews.Add(descriptor);
            }
        }

        public NavigationDescriptor Activate(string viewName)
        {
            NavigationDescriptor descriptor = _navigationViews[viewName];

            if (descriptor == null)
            {
                throw new InvalidOperationException("View missing: " + viewName);
            }

            _activeNavigationViews.Add(descriptor);
            return descriptor;
        }

        #endregion

        private class NavigationViewCollection : ObservableCollection<NavigationDescriptor>, INavigationViewCollection
        {
            #region INavigationViewCollection Members

            public NavigationDescriptor this[string viewName]
            {
                get { return this.SingleOrDefault(x => x.ViewName == viewName); }
            }

            #endregion
        }
    }
}
