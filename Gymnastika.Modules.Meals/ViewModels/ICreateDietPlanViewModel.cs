using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ICreateDietPlanViewModel
    {
        ICreateDietPlanView View { get; set; }
        IDietPlanListViewModel DietPlanListViewModel { get; set; }
        DietPlan DietPlan { get; set; }
        DateTime CreatedDate { get; set; }
        ICommand SaveCommand { get; }
    }
}
