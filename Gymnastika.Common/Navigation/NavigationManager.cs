using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using FluidKit.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Common.Navigation
{
    public class NavigationManager : INavigationManager
    {
        public NavigationManager()
        {
            _regions = new NavigationRegionCollection();
        }

        private NavigationRegionCollection _regions;

        #region INavigationManager Members

        public void AddRegionIfMissing(string regionName, string header)
        {
            INavigationRegion region = _regions[regionName];
            if (region == null)
            {
                region = ServiceLocator.Current.GetInstance<INavigationRegion>(); 
                region.Name = regionName;
                region.Header = header;

                _regions.Add(region);
            }
        }

        public void RemoveRegion(string regionName)
        {
            throw new NotImplementedException();
        }

        public INavigationRegionCollection Regions
        {
            get { return _regions; }
        }

        #endregion
    }

    internal class NavigationRegionCollection : ObservableCollection<INavigationRegion>, INavigationRegionCollection
    {
        #region INavigationRegionCollection Members

        public INavigationRegion this[string regionName]
        {
            get { return this.SingleOrDefault(x => x.Name == regionName); }
        }

        #endregion
    }
}
