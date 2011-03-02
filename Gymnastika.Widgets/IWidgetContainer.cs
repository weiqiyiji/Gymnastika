using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public interface IWidgetContainer
    {
        FrameworkElement Target { get; set; }
        ObservableCollection<IWidgetHost> WidgetHosts { get; }
        ObservableCollection<IWidget> Widgets { get; }
        IList<IWidgetContainerBehavior> Behaviors { get; }
    }
}
