using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services
{
    public interface IFoodService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int foodId);
        SubCategory GetSubCategory(int foodId);
        IEnumerable<Food> GetFoodsByName(string foodName);
        IEnumerable<Food> GetFoodsInCount(int count);
        IEnumerable<DietPlan> GetAllSavedDietPlansOfUser(int userId);
        IEnumerable<DietPlan> GetAllRecommendedDietPlans();
        void CreateDietPlan(DietPlan dietPlan);
    }
}
