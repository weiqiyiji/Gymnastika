using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IWidgetContainerBehavior
    {
        void Attach();
        IWidgetContainer Target { get; set; }
    }
}
