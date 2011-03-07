using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IDietPlanProvider
    {
        void Create(DietPlan dietPlan);
        void Update(DietPlan dietPlan);
        IEnumerable<DietPlan> GetDietPlans(int userId);
        IEnumerable<DietPlan> GetRecommendedDietPlans();
    }
}
