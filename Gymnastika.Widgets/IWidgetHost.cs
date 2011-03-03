using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public interface IWidgetHost
    {
        int Id { get; set; }
        IWidget Widget { get; set; }
    }
}
