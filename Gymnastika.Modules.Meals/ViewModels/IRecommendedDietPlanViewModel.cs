using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IRecommendedDietPlanViewModel
    {
        IRecommendedDietPlanView View { get; set; }
        ICollection<DietPlanItemViewModel> RecommendedDietPlans { get; set; }
    }
}
