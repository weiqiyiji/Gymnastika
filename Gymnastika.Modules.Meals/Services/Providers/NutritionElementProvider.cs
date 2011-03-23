using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class NutritionElementProvider : INutritionElementProvider
    {
        private readonly IRepository<NutritionElement> _repository;

        public NutritionElementProvider(IRepository<NutritionElement> repository)
        {
            _repository = repository;
        }

        #region INutritionalElementProvider Members

        public void Create(NutritionElement nutritionalElement)
        {
            _repository.Create(nutritionalElement);
        }

        public void Update(NutritionElement nutritionalElement)
        {
            _repository.Update(nutritionalElement);
        }

        public IEnumerable<NutritionElement> GetNutritionElements(Food food)
        {
            return _repository.Fetch(ne => ne.Food == food);
        }

        public IEnumerable<NutritionElement> GetNutritionElements(Food food, int skip, int count)
        {
            return _repository.Fetch(ne => ne.Food == food, one => one.Asc(ne => ne.Id), skip, count);
        }

        #endregion
    }
}
