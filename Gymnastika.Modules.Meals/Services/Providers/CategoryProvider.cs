using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class CategoryProvider : ICategoryProvider
    {
        private readonly IRepository<Category> _repository;

        public CategoryProvider(IRepository<Category> repository)
        {
            _repository = repository;
        }

        #region ICategoryProvider Members

        public void Create(Category category)
        {
            _repository.Create(category);
        }

        public void Update(Category category)
        {
            _repository.Update(category);
        }

        public IEnumerable<Category> GetAll()
        {
            return _repository.Fetch(c => true);
        }

        public Category Get(string name)
        {
            return _repository.Get(c => c.Name == name);
        }

        #endregion
    }
}
