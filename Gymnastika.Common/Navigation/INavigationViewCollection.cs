using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Gymnastika.Common.Navigation
{
    public interface INavigationViewCollection : IEnumerable<NavigationDescriptor>, INotifyCollectionChanged
    {
        NavigationDescriptor this[string viewName] { get; }
    }
}
