using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanSubItemViewModel
    {
        public DietPlanSubItemViewModel(string mealName)
        {
            MealName = mealName;
            SubTotalCalorie = 0;
            Foods = new ObservableCollection<FoodItemViewModel>();
            IsFirstItem = false;
            IsLastItem = false;
        }

        public string MealName { get; set; }

        public bool IsFirstItem { get; set; }

        public bool IsLastItem { get; set; }

        public int SubTotalCalorie { get; set; }

        public ObservableCollection<FoodItemViewModel> Foods { get; set; }
    }
}
