using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class NutritionChartItemViewModel : NotificationObject
    {
        private decimal _dietPlanNutritionValue;
        private decimal _foodItemNutritionValue;

        public NutritionChartItemViewModel(string nutritionName)
        {
            NutritionName = nutritionName;
        }

        public string NutritionName { get; set; }

        public decimal DietPlanNutritionValue
        {
            get
            {
                return _dietPlanNutritionValue;
            }
            set
            {
                if (_dietPlanNutritionValue != value)
                {
                    _dietPlanNutritionValue = value;
                    RaisePropertyChanged("DietPlanNutritionValue");
                }
            }
        }

        public decimal FoodItemNutritionValue
        {
            get
            {
                return _foodItemNutritionValue;
            }
            set
            {
                if (_foodItemNutritionValue != value)
                {
                    _foodItemNutritionValue = value;
                    RaisePropertyChanged("FoodItemNutritionValue");
                }
            }
        }
    }
}
