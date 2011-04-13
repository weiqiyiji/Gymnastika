using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IDietPlanTaskProvider
    {
        void Create(DietPlanTask dietPlanTask);
        DietPlanTask Get(int taskId);
        IEnumerable<DietPlanTask> GetDietPlanTasks(int year, int month, int day);
    }
}
