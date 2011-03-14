using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Gymnastika.Common.Navigation
{
    public interface INavigationRegionCollection : IEnumerable<INavigationRegion>, INotifyCollectionChanged
    {
        INavigationRegion this[string regionName] { get; }
    }
}
