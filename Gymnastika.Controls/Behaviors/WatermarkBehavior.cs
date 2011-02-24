using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Gymnastika.Controls.Behaviors
{
    public static class WatermarkBehavior
    {

        public static bool GetIsEmpty(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEmptyProperty);
        }

        public static void SetIsEmpty(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEmptyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEmpty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEmptyProperty =
            DependencyProperty.RegisterAttached(
                "IsEmpty", typeof(bool), typeof(WatermarkBehavior), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsEmptyChanged));

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkBehavior), new UIPropertyMetadata(string.Empty));

        private static void OnIsEmptyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            pb.SetValue(IsEmptyProperty, string.IsNullOrEmpty(pb.Password));
            pb.PasswordChanged -= OnPasswordChanged;
            pb.PasswordChanged += OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            pb.SetValue(IsEmptyProperty, string.IsNullOrEmpty(pb.Password));
        }
    }
}
