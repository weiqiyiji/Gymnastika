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
            FoodListViewModel.CurrentSubCategory = Category.SubCategories[0];
            FoodListViewModel.SelectCategory(FoodListViewModel.CurrentSubCategory);
        }

        public Category Category { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }
    }
}
