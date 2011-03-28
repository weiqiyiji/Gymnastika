using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class TodayDietPlanWidgetModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private readonly User _currentUser;

        public TodayDietPlanWidgetModel(IFoodService foodService, 
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            _currentUser = _sessionManager.GetCurrentSession().AssociatedUser;
        }

        public DietPlan DietPlan { get; set; }

        public ITodayDietPlanViewModel TodayDietPlanViewModel { get; set; }

        public void Initialize()
        {
            using (var scope = _workEnvironment.GetWorkContextScope())
            {
                DietPlan = _foodService.DietPlanProvider.Get(_currentUser, DateTime.Today);
                if (DietPlan != null)
                {
                    DietPlan.SubDietPlans = DietPlan.SubDietPlans.ToList();
                    foreach (var subDietPlan in DietPlan.SubDietPlans)
                    {
                        subDietPlan.DietPlanItems = subDietPlan.DietPlanItems.ToList();
                        foreach (var dietPlanItem in subDietPlan.DietPlanItems)
                        {
                            dietPlanItem.Food = _foodService.FoodProvider.Get(dietPlanItem);
                        }
                    }
                }
            }
            if (DietPlan != null)
            {
                TodayDietPlanViewModel = ServiceLocator.Current.GetInstance<ITodayDietPlanViewModel>();
                TodayDietPlanViewModel.DietPlan = DietPlan;
                TodayDietPlanViewModel.Initialize();
            }
        }
    }
}
