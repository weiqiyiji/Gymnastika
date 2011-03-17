using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ICategoryItemViewModel
    {
        ICategoryItemView View { get; set; }
        Category Category { get; set; }
        string ImageUri { get; }
        string Name { get; }
        IEnumerable<SubCategory> SubCategoryItems { get; set; }
        SubCategory SelectedSubCategoryItem { get; set; }
    }
}
