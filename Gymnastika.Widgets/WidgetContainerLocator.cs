using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets
{
    public static class WidgetContainerLocator
    {
        public static bool GetIsContainer(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsContainerProperty);
        }

        public static void SetIsContainer(DependencyObject obj, bool value)
        {
            obj.SetValue(IsContainerProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsContainerProperty =
            DependencyProperty.RegisterAttached("IsContainer", typeof(bool), typeof(WidgetManager), new UIPropertyMetadata(false, OnContainerSet));

        private static void OnContainerSet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement target = sender as FrameworkElement;

            if (target == null) throw new InvalidCastException("WidgetContainer should be the descendant of FrameworkElement");

            if (bool.Parse(e.NewValue.ToString()) == true)
                CreateContainer(target);
        }

        private static void CreateContainer(FrameworkElement target)
        {
            var initializer = ServiceLocator.Current.GetInstance<IWidgetContainerInitializer>();
            initializer.Initialize(target);
        }
    }
}
