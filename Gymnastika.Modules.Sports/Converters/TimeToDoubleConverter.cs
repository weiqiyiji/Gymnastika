using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Gymnastika.Modules.Sports.Converters
{
    public class TimeToDoubleConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int hour = (int)values[0];
                int minute = (int)values[1];
                return (double)hour + (double)(minute / 60);
            }
            catch (Exception)
            {
                return 0d;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                object[] times = new object[2];
                double time = (double)value;
                int hour = (int)time;
                int minute = (int)((time - (double)hour) * 60);
                times[0] = hour;
                times[1] = minute;
                return times;
            }
            catch (Exception)
            {
                return new object[2];
            }
        }

        #endregion
    }
}
