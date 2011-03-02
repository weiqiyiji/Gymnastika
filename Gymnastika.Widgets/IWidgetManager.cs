using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IWidgetManager
    {
        ObservableCollection<WidgetDescriptor> Descriptors { get; }
        void Add(Type widgetType);
        void Remove(Type widgetType);
    }
}
