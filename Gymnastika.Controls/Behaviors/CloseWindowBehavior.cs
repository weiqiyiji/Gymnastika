using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Controls.Behaviors
{
    public static class CloseWindowBehavior
    {
        public static bool GetCloseCondition(Window obj)
        {
            return (bool)obj.GetValue(CloseConditionProperty);
        }

        public static void SetCloseCondition(Window obj, bool value)
        {
            obj.SetValue(CloseConditionProperty, value);
        }

        public static readonly DependencyProperty CloseConditionProperty =
            DependencyProperty.RegisterAttached("CloseCondition", typeof(bool), typeof(CloseWindowBehavior), new UIPropertyMetadata(false, OnCloseConditionChanged));

        private static void OnCloseConditionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (bool.Parse(e.NewValue.ToString()))
            {
                Window win = (Window)sender;
                win.Close();
            }
        }
    }
}
