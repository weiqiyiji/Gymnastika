using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ICreateDietPlanViewModel
    {
        ICreateDietPlanView View { get; set; }
        IDietPlanListViewModel DietPlanListViewModel { get; set; }
        ICommand SaveCommand { get; }
    }
}
