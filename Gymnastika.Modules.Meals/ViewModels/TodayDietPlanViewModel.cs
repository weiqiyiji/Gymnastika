using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Common.Navigation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class TodayDietPlanViewModel : ITodayDietPlanViewModel
    {
        public TodayDietPlanViewModel(ITodayDietPlanView view)
        {
            View = view;
            View.Context = this;
        }

        public void Initialize()
        {
            TotalCalorie = 0;
            IList<string> mealNames = new List<string> { "早餐", "中餐", "晚餐" };
            DietPlanSubItems = new ObservableCollection<DietPlanSubItemViewModel>();
            for (int i = 0; i < 3; i++)
            {
                DietPlanSubItemViewModel dietPlanSubItem = new DietPlanSubItemViewModel(mealNames[i]);
                if (i == 0) dietPlanSubItem.IsFirstItem = true;
                if (i == 2) dietPlanSubItem.IsLastItem = true;
                foreach (var dietPlanItem in DietPlan.SubDietPlans[i].DietPlanItems)
                {
                    FoodItemViewModel foodItem = new FoodItemViewModel(dietPlanItem.Food)
                    {
                        Number = dietPlanItem.Amount
                    };
                    dietPlanSubItem.Foods.Add(foodItem);
                    dietPlanSubItem.SubTotalCalorie += (int)foodItem.Calories;
                }
                DietPlanSubItems.Add(dietPlanSubItem);
                TotalCalorie += dietPlanSubItem.SubTotalCalorie;
            }
        }

        public ITodayDietPlanView View { get; set; }

        public DietPlan DietPlan { get; set; }

        public ObservableCollection<DietPlanSubItemViewModel> DietPlanSubItems { get; set; }

        public int TotalCalorie { get; set; }

        private ICommand _modifyCommand;
        public ICommand ModifyCommand
        {
            get
            {
                if (_modifyCommand == null)
                    _modifyCommand = new DelegateCommand(Modify);

                return _modifyCommand;
            }
        }

        private void Modify()
        {
            INavigationService navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.RequestNavigate(NavigationNames.ShellRegion, NavigationNames.CreateDietPlanView);

            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ApplyRecommendedDietPlanEvent>().Publish(DietPlan);
        }
    }
}
