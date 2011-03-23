using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Meals.Events;
using Gymnastika.Common.Navigation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanItemViewModel
    {
        public DietPlanItemViewModel(DietPlan dietPlan)
        {
            DietPlan = dietPlan;
            DietPlanType = DietPlan.PlanType;
            CreatedDate = DietPlan.CreatedDate.ToString("yyyy-MM-dd");
            TotalCalorie = 0;
            IList<string> mealNames = new List<string> { "早餐", "中餐", "晚餐" };

            DietPlanSubItems = new List<DietPlanSubItemViewModel>(3);
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
            //DietPlanName = DietPlan.Name;

            if (DietPlanType == PlanType.CreatedDietPlan && CreatedDate.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) >= 0)
                ButtonContent = "修改";
            else
                ButtonContent = "应用";
        }

        public DietPlan DietPlan { get; set; }

        public IList<DietPlanSubItemViewModel> DietPlanSubItems { get; set; }

        public int TotalCalorie { get; set; }

        public string DietPlanName { get; set; }

        public string CreatedDate { get; set; }

        public PlanType DietPlanType { get; set; }

        public string ButtonContent { get; set; }

        private ICommand _applyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                if (_applyCommand == null)
                    _applyCommand = new DelegateCommand(OnApply);

                return _applyCommand;
            }
        }

        private void OnApply()
        {
            INavigationService navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.RequestNavigate(NavigationNames.ShellRegion, NavigationNames.CreateDietPlanView);

            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ApplyRecommendedDietPlanEvent>().Publish(DietPlan);
        }
    }
}
