using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanListViewModel : NotificationObject, IDietPlanListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private decimal _totalCalories;

        public DietPlanListViewModel(IDietPlanListView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.Context = this;

            TotalCalories = 0;
            CanAnimated = true;
            IList<string> mealNames = new List<string> { "早餐", "中餐", "晚餐" };

            DietPlanList = new List<IDietPlanSubListViewModel>(3);
            for (int i = 0; i < 3; i++)
            {
                IDietPlanSubListViewModel dietPlanSubList = ServiceLocator.Current.GetInstance<IDietPlanSubListViewModel>();
                dietPlanSubList.MealName = mealNames[i];
                dietPlanSubList.DietPlanListPropertyChanged += new EventHandler(DietPlanListPropertyChanged);
                DietPlanList.Add(dietPlanSubList);
            }
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ApplyRecommendedDietPlanEvent>().Subscribe(ApplyRecommendedDietPlanEventHandler);
        }

        #region IDietPlanViewModel Members

        public IDietPlanListView View { get; set; }

        public decimal TotalCalories
        {
            get
            {
                return _totalCalories;
            }
            set
            {
                if (_totalCalories != value)
                {
                    _totalCalories = value;
                    RaisePropertyChanged("TotalCalories");
                }
            }
        }

        public IList<IDietPlanSubListViewModel> DietPlanList { get; set; }

        public IList<NutritionElement> Nutritions { get; set; }

        public DietPlan DietPlan { get; set; }

        public event EventHandler DietPlanNutritionChanged;

        public bool CanAnimated { get; set; }

        #endregion

        private void DietPlanListPropertyChanged(object sender, EventArgs e)
        {
            decimal totalCalories = 0;

            foreach (var dietPlanSubList in DietPlanList)
            {
                totalCalories += dietPlanSubList.SubTotalCalories;
            }

            TotalCalories = totalCalories;

            Nutritions = new List<NutritionElement>();

            bool canAdd = false;
            for (int i = 0; i < DietPlanList.Count; i++)
            {
                if (!canAdd)
                {
                    if (DietPlanList[i].Nutritions == null) continue;
                    for (int j = 0; j < DietPlanList[i].Nutritions.Count; j++)
                    {
                        Nutritions.Add(new NutritionElement
                        {
                            Name = DietPlanList[i].Nutritions[j].Name,
                            Value = DietPlanList[i].Nutritions[j].Value
                        });
                    }
                    canAdd = true;
                }
                else
                {
                    if (DietPlanList[i].Nutritions == null) continue;
                    for (int j = 0; j < DietPlanList[i].Nutritions.Count; j++)
                    {
                        Nutritions[j].Value += DietPlanList[i].Nutritions[j].Value;
                    }
                }
            }

            if (CanAnimated)
                _eventAggregator.GetEvent<DietPlanNutritionChangedEvent>().Publish(Nutritions);
                //OnDietPlanNutritionChanged();
        }

        private void OnDietPlanNutritionChanged()
        {
            if (DietPlanNutritionChanged != null)
                DietPlanNutritionChanged(this, new EventArgs());
        }

        private void ApplyRecommendedDietPlanEventHandler(DietPlan dietPlan)
        {
            DietPlan = dietPlan;
            CanAnimated = false;
            for (int i = 0; i < 3; i++)
            {
                DietPlanList[i].FoodItems.Clear();
                foreach (var dietPlanItem in DietPlan.SubDietPlans[i].DietPlanItems)
                {
                    FoodItemViewModel foodItem = new FoodItemViewModel(dietPlanItem.Food);
                    DietPlanList[i].AddFoodToPlan(foodItem);
                    foodItem.LoadNutritionElementData();
                    foodItem.Amount = dietPlanItem.Amount;
                }
            }
            _eventAggregator.GetEvent<DietPlanNutritionChangedEvent>().Publish(Nutritions);
            //OnDietPlanNutritionChanged();
            CanAnimated = true;
        }
    }
}
