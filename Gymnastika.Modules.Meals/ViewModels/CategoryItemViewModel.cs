using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class CategoryItemViewModel
    {
        public CategoryItemViewModel(Category category)
        {
            Category = category;
            FoodListViewModel = ServiceLocator.Current.GetInstance<IFoodListViewModel>();
            FoodListViewModel.CurrentCategory = Category;
            FoodListViewModel.SelectCategory(FoodListViewModel.CurrentCategory);
        }

        public Category Category { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }
    }
}
