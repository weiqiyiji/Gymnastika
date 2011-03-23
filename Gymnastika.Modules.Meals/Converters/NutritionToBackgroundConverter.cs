using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Gymnastika.Modules.Meals.Converters
{
    public class NutritionToBackgroundConverter : IValueConverter
    {

        public Brush CalorieBrush { get; set; }

        public Brush CarbohydrateBrush { get; set; }

        public Brush FatBrush { get; set; }

        public Brush ProteinBrush { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string nutritionName = (string)value;

            switch (nutritionName)
            {
                case "热量(大卡)":
                        return CalorieBrush;
                case "碳水化合物(克)":
                        return CarbohydrateBrush;
                case "脂肪(克)":
                        return FatBrush;
                case "蛋白质(克)":
                        return ProteinBrush;
                default:
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
