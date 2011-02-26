using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public interface IWidgetContainerAdapter
    {
        void Adapt(FrameworkElement target);
    }
}
