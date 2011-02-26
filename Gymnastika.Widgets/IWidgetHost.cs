using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public enum WidgetState
    {
        Expanded,
        Collapsed
    }

    public interface IWidgetHost
    {
        int Id { get; set; }
        void Expand();
        void Collapse();
        WidgetState State { get; }
        IWidget Widget { get; set; }
        bool IsActive { get; set; }
        event EventHandler IsActiveChanged;
    }
}
