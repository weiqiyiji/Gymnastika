using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Gymnastika.Controls.Behaviors
{
    public class ClipBehavior
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ClipBehavior), new UIPropertyMetadata(new CornerRadius(0.0)));

        public static GeometryProvider GetProvider(DependencyObject obj)
        {
            return (GeometryProvider)obj.GetValue(ProviderProperty);
        }

        public static void SetProvider(DependencyObject obj, GeometryProvider value)
        {
            obj.SetValue(ProviderProperty, value);
        }

        public static readonly DependencyProperty ProviderProperty =
            DependencyProperty.RegisterAttached("Provider", typeof(GeometryProvider), typeof(ClipBehavior), new UIPropertyMetadata(null, OnSetProvider));

        public static void OnSetProvider(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement target = (FrameworkElement)sender;
            target.SizeChanged += new SizeChangedEventHandler(target_SizeChanged);
        }

        private static void target_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement target = (FrameworkElement)sender;
            GeometryProvider provider = GetProvider(target);
            Geometry clip = provider.GetGeometry(target);
            target.Clip = clip;
        }
    }
}
