using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gymnastika.Controls.Behaviors
{
    public static class PasswordBindingBehavior
    {
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password", 
                typeof(string), 
                typeof(PasswordBindingBehavior), 
                new FrameworkPropertyMetadata(
                    string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordBindingChanged));


        public static bool GetIsUpdated(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsUpdatedProperty);
        }

        public static void SetIsUpdated(DependencyObject obj, bool value)
        {
            obj.SetValue(IsUpdatedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsUpdated.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUpdatedProperty =
            DependencyProperty.RegisterAttached("IsUpdated", typeof(bool), typeof(PasswordBindingBehavior), new UIPropertyMetadata(false));

        
        private static void OnPasswordBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pb = (PasswordBox) sender;
            pb.PasswordChanged -= OnPasswordChanged;

            if (e.NewValue != null)
            {
                if (!GetIsUpdated(pb))
                {
                    pb.Password = e.NewValue.ToString();
                }
            }

            pb.PasswordChanged += OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            SetIsUpdated(pb, true);
            SetPassword(pb, pb.Password);
            SetIsUpdated(pb, false);
        }
    }
}
