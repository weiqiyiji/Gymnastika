using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IMealsManagementViewModel
    {
        IMealsManagementView View { get; set; }
        string SearchString { get; set; }
        ICommand SearchCommand { get; }
        ICommand ShowSavedDietPlanCommand { get; }
        ICommand ShowRecommendedDietPlanCommand { get; }
        IEnumerable<Food> InMemoryFoods { get; set; }
        ICollection<Food> SearchResults { get; set; }
        IFoodListViewModel FoodListViewModel { get; set; }
        ICreateDietPlanViewModel CreateDietPlanViewModel { get; set; }
        ISelectDietPlanViewModel SavedDietPlanViewModel { get; set; }
        ISelectDietPlanViewModel RecommendedDietPlanViewModel { get; set; }
    }
}
