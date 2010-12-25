using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Gymnastika.Controls.Desktop
{
    public class GlassButtonChrome : ContentControl
    {
        static GlassButtonChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GlassButtonChrome), new FrameworkPropertyMetadata(typeof(GlassButtonChrome)));
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GlassButtonChrome), new UIPropertyMetadata(new CornerRadius(0)));

        
    }
}
