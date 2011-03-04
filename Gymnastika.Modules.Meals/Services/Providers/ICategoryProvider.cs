using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface ICategoryProvider
    {
        void Create(Category category);
        void Update(Category category);
        IEnumerable<Category> GetAll();
        Category Get(string name);
    }
}
