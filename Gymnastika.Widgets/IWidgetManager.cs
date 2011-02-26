using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IWidgetManager
    {
        ReadOnlyCollection<IWidget> Widgets { get; }
        void Add(IWidget widget);
        void Remove(IWidget widget);
    }
}
