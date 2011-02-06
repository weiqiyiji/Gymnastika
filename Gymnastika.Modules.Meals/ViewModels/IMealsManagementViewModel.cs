using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IMealsManagementViewModel
    {
        IMealsManagementView View { get; set; }
        string SearchString { get; set; }
        ICommand SearchCommand { get; }
        ICommand ShowRecommendedDietPlanCommand { get; }
        IEnumerable<Food> InMemoryFoods { get; set; }
        IEnumerable<Food> SearchResults { get; set; }
        IFoodListViewModel FoodListViewModel { get; set; }
        ICreateDietPlanViewModel CreateDietPlanViewModel { get; set; }
        IRecommendDietPlanViewModel RecommendDietPlanViewModel { get; set; }
    }
}
