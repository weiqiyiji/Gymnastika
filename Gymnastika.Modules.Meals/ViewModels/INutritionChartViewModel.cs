using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface INutritionChartViewModel
    {
        INutritionChartView View { get; set; }
        IList<NutritionChartItemViewModel> NutritionChartItems { get; set; }
    }
}
