using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface ISubCategoryProvider
    {
        void Create(SubCategory subCategory);
        void Update(SubCategory subCategory);
        IEnumerable<SubCategory> GetSubCategories(Category category);
        IEnumerable<SubCategory> GetAll();
    }
}
