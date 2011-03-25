using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Gymnastika.Controls.Behaviors
{
    public abstract class GeometryProvider
    {
        public abstract Geometry GetGeometry(FrameworkElement target);
    }
}
