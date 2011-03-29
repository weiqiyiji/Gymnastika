using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IRecommendedDietPlanViewModel
    {
        IRecommendedDietPlanView View { get; set; }
        ObservableCollection<DietPlanItemViewModel> RecommendedDietPlans { get; set; }
    }
}
