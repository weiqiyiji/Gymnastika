using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface INutritionChartItemViewModel
    {
        INutritionChartItemView View { get; set; }
        string NutritionName { get; set; }
        double MinTotalNutritionValue { get; set; }
        double MaxTotalNutritionValue { get; set; }
        double DietPlanNutritionValue { get; set; }
        double OldDietPlanNutritionValue { get; set; }
        double FoodItemNutritionValue { get; set; }
        double OldFoodItemNutritionValue { get; set; }
        void BeginDietPlanAnimation();
        void BeginFoodItemAnimation();
        double OldDietPlanNutritionWidth { get; }
        double NewDietPlanNutritionWidth { get; }
        double CurrentDietPlanNutritionWidth { get; set; }
        double OldFoodItemNutritionWidth { get; }
        double NewFoodItemNutritionWidth { get; }
        double CurrentFoodItemNutritionWidth { get; set; }
    }
}
