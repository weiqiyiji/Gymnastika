using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gymnastika.Common
{
    public interface INavigationManager : INotifyCollectionChanged
    {
        event EventHandler CurrentPageChanged;
        NavigationDescriptor CurrentPage { get; set; }
        void AddIfMissing(NavigationDescriptor descriptor, bool isCurrentPage = false);
        void Remove(NavigationDescriptor descriptor);
    }
}
