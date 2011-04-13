using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Meals.Models;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class RecommendedDietPlanViewModel : IRecommendedDietPlanViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;

        public RecommendedDietPlanViewModel(IRecommendedDietPlanView view,
            IFoodService foodService,
            IWorkEnvironment workEnvironment)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;

            using (var scope = _workEnvironment.GetWorkContextScope())
            {
                DietPlans = _foodService.DietPlanProvider.GetRecommendedDietPlans();
                foreach (var dietPlan in DietPlans)
                {
                    dietPlan.SubDietPlans = dietPlan.SubDietPlans.ToList();
                    foreach (var subDietPlan in dietPlan.SubDietPlans)
                    {
                        subDietPlan.DietPlanItems = subDietPlan.DietPlanItems.ToList();
                        foreach (var dietPlanItem in subDietPlan.DietPlanItems)
                        {
                            dietPlanItem.Food = _foodService.FoodProvider.Get(dietPlanItem);
                        }
                    }
                }
            }
            RecommendedDietPlans = new ObservableCollection<DietPlanItemViewModel>();
            foreach (var dietPlan in DietPlans)
            {
                DietPlanItemViewModel dietPlanItem = new DietPlanItemViewModel(dietPlan);
                RecommendedDietPlans.Add(dietPlanItem);
            }
            View = view;
            View.Context = this;
        }

        #region IRecommendedDietPlanViewModel Members

        public IRecommendedDietPlanView View { get; set; }

        public IEnumerable<DietPlan> DietPlans { get; set; }

        public ObservableCollection<DietPlanItemViewModel> RecommendedDietPlans { get; set; }

        #endregion
    }
}
