using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluidKit.Controls;

namespace Gymnastika.Common.Navigation
{
    public delegate void NavigationHandler(object sender, NavigationEventArgs e);

    public class NavigationEventArgs : EventArgs
    {
        public NavigationDescriptor SourceDescriptor { get; set; }

        public NavigationDescriptor TargetDescriptor { get; set; }

        public NavigationEventArgs(NavigationDescriptor sourceDescriptor, NavigationDescriptor targetDescriptor)
        {
            SourceDescriptor = sourceDescriptor;
            TargetDescriptor = targetDescriptor;
        }
    }

    public interface INavigationService
    {
        void RequestNavigate(string regionName, string viewName);
        event NavigationHandler NavigationStart;
        event NavigationHandler NavigationCompleted;
    }
}
