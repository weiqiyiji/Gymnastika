using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Communication.Services;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Sync.Communication;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class TodayDietPlanWidgetModel : NotificationObject
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly User _currentUser;
        private readonly CommunicationService _communicationService;

        public TodayDietPlanWidgetModel(IFoodService foodService, 
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager,
            IEventAggregator eventAggregator)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            _currentUser = _sessionManager.GetCurrentSession().AssociatedUser;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AddOrModifiedDietPlanEvent>().Subscribe(AddOrModifiedDietPlanEventHandler);
        }

        public DietPlan DietPlan { get; set; }


        private ITodayDietPlanViewModel _todayDietPlanViewModel;
        public ITodayDietPlanViewModel TodayDietPlanViewModel
        {
            get
            {
                return _todayDietPlanViewModel;
            }
            set
            {
                if (_todayDietPlanViewModel != value)
                {
                    _todayDietPlanViewModel = value;
                    RaisePropertyChanged("TodayDietPlanViewModel");
                }
            }
        }

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
                InitializeTodayDietPlan(DietPlan);
            }
            else
            {
                TodayDietPlanViewModel = ServiceLocator.Current.GetInstance<ITodayDietPlanViewModel>();
            }
        }

        private void AddOrModifiedDietPlanEventHandler(DietPlan dietPlan)
        {
            InitializeTodayDietPlan(dietPlan);
        }

        private void InitializeTodayDietPlan(DietPlan dietPlan)
        {
            TodayDietPlanViewModel = ServiceLocator.Current.GetInstance<ITodayDietPlanViewModel>();
            TodayDietPlanViewModel.DietPlan = dietPlan;
            TodayDietPlanViewModel.Initialize();
        }
    }
}
