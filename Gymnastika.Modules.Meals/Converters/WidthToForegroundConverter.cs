using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Gymnastika.Modules.Meals.Converters
{
    public class WidthToForegroundConverter : IMultiValueConverter
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
            double minTotalNutritionValue = (double)values[1];
            double maxTotalNutritionValue = (double)values[2];
            double dietPlanNutritionValue = (double)values[3];

            switch (nutritionName)
            {
                case "热量(大卡)":
                    if (dietPlanNutritionValue < minTotalNutritionValue)
                        return CalorieLowLevelBrush;
                    else if (dietPlanNutritionValue > maxTotalNutritionValue)
                        return CalorieHighLevelBrush;
                    else
                        return CalorieMediumLevelBrush;
                case "碳水化合物(克)":
                    if (dietPlanNutritionValue < minTotalNutritionValue)
                        return CarbohydrateLowLevelBrush;
                    else if (dietPlanNutritionValue > maxTotalNutritionValue)
                        return CarbohydrateHighLevelBrush;
                    else
                        return CarbohydrateMediumLevelBrush;
                case "脂肪(克)":
                    if (dietPlanNutritionValue < minTotalNutritionValue)
                        return FatLowLevelBrush;
                    else if (dietPlanNutritionValue > minTotalNutritionValue)
                        return FatHighLevelBrush;
                    else
                        return FatMediumLevelBrush;
                case "蛋白质(克)":
                    if (dietPlanNutritionValue < minTotalNutritionValue)
                        return ProteinLowLevelBrush;
                    else if (dietPlanNutritionValue > maxTotalNutritionValue)
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
