using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IDietPlanSubListViewModel
    {
        IDietPlanSubListView View { get; set; }
        ObservableCollection<FoodItemViewModel> DietPlanSubList { get; set; }
        string MealName { get; set; }
        decimal SubTotalCalories { get; set; }
        void AddFoodToPlan(FoodItemViewModel foodItem);
        event EventHandler DietPlanListPropertyChanged;
        IList<NutritionalElement> Nutritions { get; set; }
    }
}
