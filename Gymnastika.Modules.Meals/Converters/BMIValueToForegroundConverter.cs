using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Gymnastika.Modules.Meals.Converters
{
    public class BMIValueToForegroundConverter : IValueConverter
    {
        public Brush LowLevelBrush { get; set; }

        public Brush MiddleLevelBrush { get; set; }

        public Brush HighLevelBrush { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int currentValue = (int)value;
            if (currentValue < 18)
                return LowLevelBrush;
            else if (currentValue > 25)
                return HighLevelBrush;
            else
                return MiddleLevelBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
