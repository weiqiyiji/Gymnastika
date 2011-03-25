using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Gymnastika.Modules.Meals.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public Brush ImageBrush { get; set; }

        public Brush SolidColorBrush { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isLastItem = (bool)value;
            if (isLastItem)
                return ImageBrush;
            return SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
