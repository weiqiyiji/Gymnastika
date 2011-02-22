using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Gymnastika.Controls.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) throw new ArgumentException("value");

            bool condition = Boolean.Parse(value.ToString());

            if(condition)
                return Visibility.Visible;

            if (parameter != null && "Hidden" == parameter.ToString())
                return Visibility.Hidden;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            Visibility returnVisibility;
            if (Enum.TryParse(value.ToString(), out returnVisibility))
                return returnVisibility == Visibility.Visible;

            return false;
        }
    }
}
