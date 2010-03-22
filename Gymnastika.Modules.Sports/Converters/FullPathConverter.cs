using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;

namespace Gymnastika.Modules.Sports.Converters
{
    public class FullPathConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string basePath = Directory.GetCurrentDirectory();
                string relativePath = value.ToString();
                string fullPath = basePath + relativePath;
                return fullPath;
            }
            catch (Exception) { }
            return "";


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
