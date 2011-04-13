using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ITodayDietPlanViewModel
    {
        ITodayDietPlanView View { get; set; }
        DietPlan DietPlan { get; set; }
        void Initialize();
    }
}
