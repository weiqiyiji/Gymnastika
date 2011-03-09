using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IFoodProvider
    {
        void Create(Food food);
        void Update(Food food);
        IEnumerable<Food> GetFoods(string name);
        IEnumerable<Food> GetAll();
        Food Get(string name);
        IEnumerable<Food> GetFoods(SubCategory subCategory);
        IEnumerable<Food> GetFoods(FavoriteFood favoriteFood);
        Food Get(DietPlanItem dietPlanItem);
        IEnumerable<Food> GetFoods(SubCategory subCategory, int skip, int count);
        int Count(SubCategory subCategory);
    }
}
