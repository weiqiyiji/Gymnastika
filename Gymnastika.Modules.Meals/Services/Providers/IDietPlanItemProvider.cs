using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IDietPlanItemProvider
    {
        void Create(DietPlanItem dietPlanItem);
        void Update(DietPlanItem dietPlanItem);
        IEnumerable<DietPlanItem> GetDietPlanItems(SubDietPlan subDietPlan);
    }
}
