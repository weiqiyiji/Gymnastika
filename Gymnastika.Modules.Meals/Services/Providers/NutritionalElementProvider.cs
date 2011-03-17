using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class NutritionalElementProvider : INutritionalElementProvider
    {
        private readonly IRepository<NutritionalElement> _repository;

        public NutritionalElementProvider(IRepository<NutritionalElement> repository)
        {
            _repository = repository;
        }

        #region INutritionalElementProvider Members

        public void Create(NutritionalElement nutritionalElement)
        {
            _repository.Create(nutritionalElement);
        }

        public void Update(NutritionalElement nutritionalElement)
        {
            _repository.Update(nutritionalElement);
        }

        public IEnumerable<NutritionalElement> GetNutritionalElements(Food food)
        {
            return _repository.Fetch(ne => ne.Food == food);
        }

        public IEnumerable<NutritionalElement> GetNutritionalElements(Food food, int skip, int count)
        {
            return _repository.Fetch(ne => ne.Food == food, one => one.Asc(ne => ne.Id), skip, count);
        }

        #endregion
    }
}
