using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IWidgetContainerAccessor
    {
        event EventHandler ContainerReady;
        IWidgetContainer Container { get; set; }
    }
}
