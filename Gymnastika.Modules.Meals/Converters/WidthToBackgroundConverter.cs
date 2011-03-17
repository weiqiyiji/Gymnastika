using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Gymnastika.Modules.Meals.Converters
{
    public class WidthToBackgroundConverter : IMultiValueConverter
    {
        public Brush CalorieLowLevelBrush { get; set; }

        public Brush CalorieMediumLevelBrush { get; set; }

        public Brush CalorieHighLevelBrush { get; set; }


        public Brush CarbohydrateLowLevelBrush { get; set; }

        public Brush CarbohydrateMediumLevelBrush { get; set; }

        public Brush CarbohydrateHighLevelBrush { get; set; }


        public Brush FatLowLevelBrush { get; set; }

        public Brush FatMediumLevelBrush { get; set; }

        public Brush FatHighLevelBrush { get; set; }


        public Brush ProteinLowLevelBrush { get; set; }

        public Brush ProteinMediumLevelBrush { get; set; }

        public Brush ProteinHighLevelBrush { get; set; }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string nutritionName = (string)values[0];
            double width = (double)values[1];

            switch (nutritionName)
            {
                case "热量(大卡)":
                    if (width < 150)
                        return CalorieLowLevelBrush;
                    else if (width > 195)
                        return CalorieHighLevelBrush;
                    else
                        return CalorieMediumLevelBrush;
                case "碳水化合物(克)":
                    if (width < 150)
                        return CarbohydrateLowLevelBrush;
                    else if (width > 195)
                        return CarbohydrateHighLevelBrush;
                    else
                        return CarbohydrateMediumLevelBrush;
                case "脂肪(克)":
                    if (width < 150)
                        return FatLowLevelBrush;
                    else if (width > 195)
                        return FatHighLevelBrush;
                    else
                        return FatMediumLevelBrush;
                case "蛋白质(克)":
                    if (width < 150)
                        return ProteinLowLevelBrush;
                    else if (width > 195)
                        return ProteinHighLevelBrush;
                    else
                        return ProteinMediumLevelBrush;
                default:
                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
