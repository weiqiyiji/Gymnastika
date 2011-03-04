using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Gymnastika.Modules.Sports.Converters
{
    public class CalorieDecriptionConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values[0]==null)
            {
                values[0] = "";
            }
            if (values[1] == null)
            {
                values[1] = "";
            }
            return String.Format("消耗热量: 每 {0} 分钟  消耗 {1} 大卡", values[0], values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
