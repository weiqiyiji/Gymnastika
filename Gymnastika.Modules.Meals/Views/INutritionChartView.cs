using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.ViewModels;
using System.Windows;
using System.Windows.Media.Animation;

namespace Gymnastika.Modules.Meals.Views
{
    public interface INutritionChartView
    {
        INutritionChartViewModel Context { get; set; }
    }
}
