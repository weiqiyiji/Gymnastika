using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IDietPlanListViewModel
    {
        IDietPlanListView View { get; set; }
        decimal TotalCalories { get; set; }
        IList<IDietPlanSubListViewModel> DietPlanList { get; set; }
        IList<NutritionalElement> Nutritions { get; set; }
    }
}
