using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Navigation
{
    public interface INavigationRegion
    {
        string Name { get; set; }
        string Header { get; set; }
        INavigationViewCollection NavigationViews { get; }
        INavigationViewCollection ActiveNavigationViews { get; }
        void Add(NavigationDescriptor descriptor);
        NavigationDescriptor Activate(string viewName);
    }
}
