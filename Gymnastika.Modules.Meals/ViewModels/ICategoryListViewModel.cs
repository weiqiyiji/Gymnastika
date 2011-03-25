using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ICategoryListViewModel
    {
        ICategoryListView View { get; set; }
        IList<CategoryItemViewModel> CategoryItems { get; set; }
        Category SelectedCategoryItem { get; set; }
        IEnumerable<Category> Categories { get; set; }
        IFoodListViewModel FoodListViewModel { get; set; }
    }
}
