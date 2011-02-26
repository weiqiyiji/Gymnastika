using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public class WidgetContext
    {
        public IWidgetHost Host { get; set; }
        public IWidgetContainer Container { get; set; }
    }
}
