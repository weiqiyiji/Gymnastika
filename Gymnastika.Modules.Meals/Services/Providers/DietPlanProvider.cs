using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Services.Models;

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

        public DietPlan Get(User user, int skip)
        {
            return _repository.Fetch(dp => dp.User == user, odp => odp.Desc(d => d.Id), skip, 1).ToList()[0];
        }

        public DietPlan Get(PlanType planType, int skip)
        {
            return _repository.Fetch(dp => dp.PlanType == planType, odp => odp.Asc(d => d.Id), skip, 1).ToList()[0];
        }

        public int count(User user)
        {
            return _repository.Count(dp => dp.User == user);
        }

        public int count(PlanType planType)
        {
            return _repository.Count(dp => dp.PlanType == planType);
        }

        #endregion
    }
}
