using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IRecommendDietPlanViewModel
    {
        IRecommendDietPlanView View { get; set; }
        IDietPlanListViewModel DietPlanListViewModel { get; set; }
        IList<RecommendedDietPlan> InMemoryRecommendedDietPlan { get; set; }
        IList<DietPlanSubListViewModel> CurrentPageDietPlanList { get; set; }
        IList<DietPlanSubListViewModel> PreviousPageDietPlanList { get; set; }
        IList<DietPlanSubListViewModel> NextPageDietPlanList { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        ICommand ShowPreviousPageCommand { get; }
        ICommand ShowNextPageCommand { get; }
        ICommand ApplyCommand { get; }
        event EventHandler Apply;
    }
}
