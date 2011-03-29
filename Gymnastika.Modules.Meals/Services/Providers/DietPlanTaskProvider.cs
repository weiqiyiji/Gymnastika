using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class DietPlanTaskProvider : IDietPlanTaskProvider
    {
        private readonly IRepository<DietPlanTask> _repository;

        public DietPlanTaskProvider(IRepository<DietPlanTask> repository)
        {
            _repository = repository;
        }

        #region IDietPlanTaskProvider Members

        public void Create(DietPlanTask dietPlanTask)
        {
            _repository.Create(dietPlanTask);
        }

        public DietPlanTask Get(int taskId)
        {
            return _repository.Get(taskId);
        }

        public IEnumerable<DietPlanTask> GetDietPlanTasks(int year, int month, int day)
        {
            return _repository.Fetch(dp => (dp.Year == year && dp.Month == year && dp.Day == day));
        }

        #endregion
    }
}
