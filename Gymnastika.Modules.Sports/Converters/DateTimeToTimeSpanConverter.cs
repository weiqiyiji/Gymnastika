using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Gymnastika.Modules.Sports.Converters
{
    public class DateTimeToTimeSpanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime datetime = (DateTime)value;
            TimeSpan timespan = new TimeSpan(datetime.Hour, datetime.Minute, datetime.Second);
            return timespan;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan timespan = (TimeSpan)value;
            DateTime datetime = new DateTime(2000, 1, 15, timespan.Hours, timespan.Minutes, timespan.Seconds);
            return datetime;
        }

        #endregion
    }
}
