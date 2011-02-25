using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IFoodDetailViewModel
    {
        IFoodDetailView View { get; set; }
        Food Food { get; set; }
        string Name { get; }
        string ImageUri { get; }
        string Calorie { get; }
        string CategoryName { get; }
        string SubCategoryName { get; }
        IEnumerable<NutritionalElement> NutritionalElements { get; }
        IEnumerable<Introduction> Introductions { get; }
    }
}
