using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Meals.Models;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanSubListViewModel : NotificationObject, IDropTarget, IDietPlanSubListViewModel
    {
        private decimal _subTotalCalories;

        public DietPlanSubListViewModel(IDietPlanSubListView view)
        {
            _subTotalCalories = 0;
            DietPlanSubList = new ObservableCollection<FoodItemViewModel>();
            View = view;
        }

        public IDietPlanSubListView View { get; set; }

        public ObservableCollection<FoodItemViewModel> DietPlanSubList { get; set; }

        public string MealName { get; set; }

        public decimal SubTotalCalories
        {
            get
            {
                return _subTotalCalories;
            }
            set
            {
                if (_subTotalCalories != value)
                {
                    _subTotalCalories = value;
                    RaisePropertyChanged("SubTotalCalories");
                    //OnDietPlanListPropertyChanged();
                }
            }
        }

        public void AddFoodToPlan(FoodItemViewModel foodItem)
        {
            Refresh(foodItem);

            DietPlanSubList.Add(foodItem);

            SubTotalCalories += foodItem.Calories;
        }

        public event EventHandler DietPlanListPropertyChanged;

        public IList<NutritionalElement> Nutritions { get; set; }

        #region IDropTarget Members

        void IDropTarget.DragOver(DropInfo dropInfo)
        {
            if (dropInfo.Data is FoodItemViewModel || dropInfo.Data is IEnumerable<FoodItemViewModel>)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(DropInfo dropInfo)
        {
            FoodItemViewModel foodItem = new FoodItemViewModel(((FoodItemViewModel)dropInfo.Data).Food); ;

            ObservableCollection<FoodItemViewModel> targetDietPlanSubList = (ObservableCollection<FoodItemViewModel>)dropInfo.TargetCollection;
            
            FoodItemViewModel targetFoodItem = targetDietPlanSubList.FirstOrDefault(f => f.Name == foodItem.Name);
            if (targetFoodItem == null)
            {
                targetDietPlanSubList.Add(foodItem);
                Refresh(foodItem);
                SubTotalCalories += foodItem.Calories;

                Nutritions = new List<NutritionalElement>();

                for (int i = 0; i < DietPlanSubList.Count; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < DietPlanSubList[i].Nutritions.Count; j++)
                        {
                            Nutritions.Add(new NutritionalElement
                            {
                                Name = DietPlanSubList[i].Nutritions[j].Name,
                                Value = DietPlanSubList[i].Nutritions[j].Value
                            });
                        }
                    }
                    else
                    {
                        if (DietPlanSubList[i].Nutritions == null) continue;
                        for (int j = 0; j < DietPlanSubList[i].Nutritions.Count; j++)
                        {
                            Nutritions[j].Value += DietPlanSubList[i].Nutritions[j].Value;
                        }
                    }
                }

                OnDietPlanListPropertyChanged();
            }
            else
            {
                targetFoodItem.Amount += 100;
            }
        }

        #endregion

        private void DeleteFoodFromPlan(object sender, EventArgs e)
        {
            FoodItemViewModel foodItem = sender as FoodItemViewModel;

            DeleteFoodFromPlan(foodItem);
        }

        private void DeleteFoodFromPlan(FoodItemViewModel foodItem)
        {
            foodItem.DeleteFoodFromPlan -= DeleteFoodFromPlan;
            foodItem.DietPlanSubListPropertyChanged -= DietPlanSubListPropertyChanged;

            DietPlanSubList.Remove(foodItem);

            SubTotalCalories -= foodItem.Calories;

            for (int i = 0; i < foodItem.Nutritions.Count; i++)
            {
                Nutritions[i].Value -= foodItem.Nutritions[i].Value;
            }
        }

        private void OnDietPlanListPropertyChanged()
        {
            if (DietPlanListPropertyChanged != null)
                DietPlanListPropertyChanged(this, new EventArgs());
        }

        private void DietPlanSubListPropertyChanged(object sender, EventArgs e)
        {
            decimal subTotalCalories = 0;

            foreach (var foodItem in DietPlanSubList)
            {
                subTotalCalories += foodItem.Calories;
            }

            SubTotalCalories = subTotalCalories;

            Nutritions = new List<NutritionalElement>();

            for (int i = 0; i < DietPlanSubList.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < DietPlanSubList[i].Nutritions.Count; j++)
                    {
                        Nutritions.Add(new NutritionalElement
                        {
                            Name = DietPlanSubList[i].Nutritions[j].Name,
                            Value = DietPlanSubList[i].Nutritions[j].Value
                        });
                    }
                }
                else
                {
                    for (int j = 0; j < DietPlanSubList[i].Nutritions.Count; j++)
                    {
                        Nutritions[j].Value += DietPlanSubList[i].Nutritions[j].Value;
                    }
                }
            }

            OnDietPlanListPropertyChanged();
        }

        private void Refresh(FoodItemViewModel foodItem)
        {
            foodItem.DeleteFoodFromPlan += new EventHandler(DeleteFoodFromPlan);
            foodItem.DietPlanSubListPropertyChanged += new EventHandler(DietPlanSubListPropertyChanged);
        }
    }
}
