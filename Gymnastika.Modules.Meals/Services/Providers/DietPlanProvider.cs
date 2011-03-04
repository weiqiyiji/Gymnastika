using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class DietPlanProvider : IDietPlanProvider
    {
        private readonly IRepository<DietPlan> _repository;

        public DietPlanProvider(IRepository<DietPlan> repository)
        {
            _repository = repository;
        }

        #region IDietPlanProvider Members

        public void Create(DietPlan dietPlan)
        {
            _repository.Create(dietPlan);
        }

        public void Update(DietPlan dietPlan)
        {
            _repository.Update(dietPlan);
        }

        public IEnumerable<DietPlan> GetDietPlans(int userId)
        {
            return _repository.Fetch(dp => dp.User.Id == userId);
        }

        public IEnumerable<DietPlan> GetRecommendedDietPlans()
        {
            return _repository.Fetch(dp => dp.PlanType == PlanType.RecommendedDietPlan);
        }

        #endregion
    }
}
