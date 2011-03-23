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
        DateTime CreatedDate { get; set; }
        ICommand SaveCommand { get; }
        ICommand SearchCommand { get; }
        IEnumerable<Food> InMemoryFoods { get; set; }
        ICollection<Food> SearchResults { get; set; }
        ICategoryListViewModel CategoryListViewModel { get; set; }
        IFoodListViewModel FoodListViewModel { get; set; }
        INutritionChartViewModel NutritionChartViewModel { get; set; }
        IPositionedFoodViewModel PositionedFoodViewModel { get; set; }
    }
}
