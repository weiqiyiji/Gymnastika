using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class FoodProvider : IFoodProvider
    {
        private readonly IRepository<Food> _repository;

        public FoodProvider(IRepository<Food> repository)
        {
            _repository = repository;
        }

        #region IFoodProvider Members

        public void Create(Food food)
        {
            _repository.Create(food);
        }

        public void Update(Food food)
        {
            _repository.Update(food);
        }

        public IEnumerable<Food> GetFoods(string name)
        {
            return _repository.Fetch(f => f.Name.Contains(name));
        }

        public IEnumerable<Food> GetAll()
        {
            return _repository.Fetch(f => true);
        }

        public Food Get(string name)
        {
            return _repository.Get(f => f.Name == name);
        }

        public IEnumerable<Food> GetFoods(SubCategory subCategory)
        {
            return _repository.Fetch(f => f.SubCategory == subCategory);
        }

        #endregion
    }
}
