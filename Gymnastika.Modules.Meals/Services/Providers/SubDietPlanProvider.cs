using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class SubDietPlanProvider : ISubDietPlanProvider
    {
        private readonly IRepository<SubDietPlan> _repository;

        public SubDietPlanProvider(IRepository<SubDietPlan> repository)
        {
            _repository = repository;
        }

        #region ISubDietPlanProvider Members

        public void Create(SubDietPlan subDietPlan)
        {
            _repository.Create(subDietPlan);
        }

        public void Update(SubDietPlan subDietPlan)
        {
            _repository.Update(subDietPlan);
        }

        public IEnumerable<SubDietPlan> GetSubDietPlans(DietPlan dietPlan)
        {
            return _repository.Fetch(sdp => sdp.DietPlan == dietPlan);
        }

        #endregion
    }
}
