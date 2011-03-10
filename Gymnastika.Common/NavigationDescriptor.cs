using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Gymnastika.Common
{
    public class NavigationDescriptor
    {
        public string RegionName { get; set; }
        public string Label { get; set; }
        public Type ViewType { get; set; }
        public string ViewName { get; set; }
    }
}
