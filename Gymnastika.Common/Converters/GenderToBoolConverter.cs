using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Converters
{
    public class GenderToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Gender gender = (Gender)value;
            return gender == (Gender)Enum.Parse(typeof(Gender), parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null) parameter = Gender.Male;

            if (value != null)
            {
                bool genderIsTrue = bool.Parse(value.ToString());
                if (genderIsTrue) return Enum.Parse(typeof(Gender), parameter.ToString());
            }

            return Gender.Male;
        }

        #endregion
    }
}
