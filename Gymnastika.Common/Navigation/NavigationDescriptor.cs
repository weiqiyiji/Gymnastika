using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace Gymnastika.Common.Navigation
{
    public class NavigationDescriptor
    {
        public NavigationDescriptor()
        {
            DelayLoad = true;
        }

        public bool DelayLoad { get; set; }
        public string Header { get; set; }
        public string ViewName { get; set; }
        public Func<FrameworkElement> ViewResolver { get; set; }
    }
}
