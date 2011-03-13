using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Controls
{
    public class ChartRowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FirstRowTemplate { get; set; }

        public DataTemplate SecondRowTemplate { get; set; }

        public DataTemplate ThirdRowTemplate { get; set; }

        public DataTemplate FourthRowTemplate { get; set; }

        public string PropertyToEvaluate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            NutritionChartItemViewModel nutritionChartItem = (NutritionChartItemViewModel)item;

            Type type = nutritionChartItem.GetType();
            PropertyInfo property = type.GetProperty(PropertyToEvaluate);

            string[] PropertyValues = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };

            string PropertyValue = property.GetValue(nutritionChartItem, null).ToString();

            if (PropertyValue == PropertyValues[0])
            {
                return FirstRowTemplate;
            }
            if (PropertyValue == PropertyValues[1])
            {
                return SecondRowTemplate;
            }
            if (PropertyValue == PropertyValues[2])
            {
                return ThirdRowTemplate;
            }
            if (PropertyValue == PropertyValues[3])
            {
                return FourthRowTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
