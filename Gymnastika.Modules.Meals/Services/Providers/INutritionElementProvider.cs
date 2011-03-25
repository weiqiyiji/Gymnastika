using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface INutritionElementProvider
    {
        void Create(NutritionElement nutritionalElement);
        void Update(NutritionElement nutritionalElement);
        IEnumerable<NutritionElement> GetNutritionElements(Food food);
        IEnumerable<NutritionElement> GetNutritionElements(Food food, int skip, int count);
    }
}
