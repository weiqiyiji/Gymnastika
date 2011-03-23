using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class HistoryDietPlanViewModel : IHistoryDietPlanViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;

        public HistoryDietPlanViewModel(IHistoryDietPlanView view,
            IFoodService foodService,
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager,
            IEventAggregator eventAggregator)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;

            using (var scope = _workEnvironment.GetWorkContextScope())
            {
                DietPlans = _foodService.DietPlanProvider.GetDietPlans(_sessionManager.GetCurrentSession().AssociatedUser.Id);
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
            HistoryDietPlans = new ObservableCollection<DietPlanItemViewModel>();
            foreach (var dietPlan in DietPlans)
            {
                DietPlanItemViewModel dietPlanItem = new DietPlanItemViewModel(dietPlan);
                HistoryDietPlans.Add(dietPlanItem);
            }
            View = view;
            View.Context = this;
            _eventAggregator.GetEvent<NotifyHistoryDietPlanChangedEvent>().Subscribe(NotifyHistoryDietPlanChangedEventHandler);
        }

        #region IHistoryDietPlanViewModel Members

        public IHistoryDietPlanView View { get; set; }

        public IEnumerable<DietPlan> DietPlans { get; set; }

        public ObservableCollection<DietPlanItemViewModel> HistoryDietPlans { get; set; }

        #endregion

        private void NotifyHistoryDietPlanChangedEventHandler(DietPlan dietPlan)
        {
            foreach (var historyDietPlan in HistoryDietPlans)
            {
                if (historyDietPlan.CreatedDate == dietPlan.CreatedDate.ToString("yyyy-MM-dd"))
                {
                    HistoryDietPlans.Remove(historyDietPlan);

                    using (var scope = _workEnvironment.GetWorkContextScope())
                    {
                        _foodService.DietPlanProvider.Delete(historyDietPlan.DietPlan);
                    }

                    break;
                }
            }
            DietPlanItemViewModel dietPlanItem = new DietPlanItemViewModel(dietPlan);
            HistoryDietPlans.Add(dietPlanItem);
        }
    }
}
