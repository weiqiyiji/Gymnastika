using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ISelectDietPlanViewModel
    {
        ISelectDietPlanView View { get; set; }
        IDietPlanListViewModel DietPlanListViewModel { get; set; }
        IList<DietPlan> InMemoryDietPlans { get; set; }
        IList<DietPlanSubListViewModel> CurrentPageDietPlanList { get; set; }
        IList<DietPlanSubListViewModel> PreviousPageDietPlanList { get; set; }
        IList<DietPlanSubListViewModel> NextPageDietPlanList { get; set; }
        PlanType PlanType { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        ICommand ShowPreviousPageCommand { get; }
        ICommand ShowNextPageCommand { get; }
        ICommand ApplyCommand { get; }
        event EventHandler Apply;
        void Initialize();
    }
}
