using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public class WidgetContainerAccessor : IWidgetContainerAccessor
    {
        public IWidgetContainer Container { get; set; }
    }
}
