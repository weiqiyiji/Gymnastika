using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IDietPlanNutritionChartViewModel
    {
        IDietPlanNutritionChartView View { get; set; }
        IList<DietPlanNutritionChartItemViewModel> DietPlanNutritionChartItems { get; set; }
        void DietPlanNutritionChangedHandler(IList<NutritionalElement> nutritions);
    }
}
