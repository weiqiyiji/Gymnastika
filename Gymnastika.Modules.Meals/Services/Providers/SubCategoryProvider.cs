using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class SubCategoryProvider : ISubCategoryProvider
    {
        private readonly IRepository<SubCategory> _repository;

        public SubCategoryProvider(IRepository<SubCategory> repository)
        {
            _repository = repository;
        }

        #region ISubCategoryProvider Members

        public void Create(SubCategory subCategory)
        {
            _repository.Create(subCategory);
        }

        public void Update(SubCategory subCategory)
        {
            _repository.Update(subCategory);
        }

        public IEnumerable<SubCategory> GetSubCategories(Category category)
        {
            return _repository.Fetch(sc => sc.Category == category);
        }

        public IEnumerable<SubCategory> GetAll()
        {
            return _repository.Fetch(sc => true);
        }

        #endregion
    }
}
