using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class DietPlanItemProvider : IDietPlanItemProvider
    {
        private readonly IRepository<DietPlanItem> _repository;

        public DietPlanItemProvider(IRepository<DietPlanItem> repository)
        {
            _repository = repository;
        }

        #region IDietPlanItemProvider Members

        public void Create(DietPlanItem dietPlanItem)
        {
            _repository.Create(dietPlanItem);
        }

        public void Update(DietPlanItem dietPlanItem)
        {
            _repository.Update(dietPlanItem);
        }

        #endregion
    }
}
