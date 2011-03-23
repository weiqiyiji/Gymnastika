using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Gymnastika.Modules.Sports.Facilities;

namespace Gymnastika.Modules.Sports.Converters
{
    public class DoubleTimeToString : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double time = (double)value;
                int hour = (int)time;
                int minute = (int)(((double)time - hour) * 60);
                return DateFacility.GetShortTime(hour, minute);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
