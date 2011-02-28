using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public class WidgetMetadataAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Icon { get; set; }

        public WidgetMetadataAttribute(string displayName, string icon)
        {
            DisplayName = displayName;
            Icon = icon;
        }
    }
}
