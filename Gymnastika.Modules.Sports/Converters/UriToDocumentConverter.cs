using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.IO;

namespace Gymnastika.Modules.Sports.Converters
{
    //[ValueConversion(typeof(string), typeof(FlowDocument))]
    public class UriToDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doc = new FlowDocument();
            string path = value as string;
            if (!string.IsNullOrEmpty(path))
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    doc = XamlReader.Load(stream) as FlowDocument;
                }
            }
            return doc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
