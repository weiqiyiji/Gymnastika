using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Meals.Models;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanSubListViewModel : NotificationObject, IDropTarget
    {
        private readonly string _mealName;
        private int _subTotalCalories;

        public DietPlanSubListViewModel(string mealName)
        {
            _mealName = mealName;
            _subTotalCalories = 0;
            DietPlanSubList = new ObservableCollection<FoodItemViewModel>();
        }

        public ObservableCollection<FoodItemViewModel> DietPlanSubList { get; set; }

        public string MealName
        {
            get { return _mealName; }
        }

        public int SubTotalCalories
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
                    OnDietPlanListPropertyChanged();
                }
            }
        }

        public event EventHandler DietPlanListPropertyChanged;

        #region IDropTarget Members

        void IDropTarget.DragOver(DropInfo dropInfo)
        {
            if ((dropInfo.Data is FoodItemViewModel || dropInfo.Data is IEnumerable<FoodItemViewModel>) && dropInfo.TargetItem is FoodItemViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(DropInfo dropInfo)
        {
            FoodItemViewModel foodItem = (FoodItemViewModel)dropInfo.Data;
            foodItem.DeleteFoodFromPlan += new EventHandler(DeleteFoodFromPlan);
            foodItem.DietPlanSubListPropertyChanged += new EventHandler(DietPlanSubListPropertyChanged);

            ObservableCollection<FoodItemViewModel> targetDietPlanSubList = (ObservableCollection<FoodItemViewModel>)dropInfo.TargetCollection;
            targetDietPlanSubList.Add(foodItem);

            SubTotalCalories += foodItem.Calories;
        }

        #endregion

        private void OnDietPlanListPropertyChanged()
        {
            if (DietPlanListPropertyChanged != null)
                DietPlanListPropertyChanged(this, new EventArgs());
        }

        private void DeleteFoodFromPlan(object sender, EventArgs e)
        {
            FoodItemViewModel foodItem = sender as FoodItemViewModel;
            foodItem.DeleteFoodFromPlan -= DeleteFoodFromPlan;
            foodItem.DietPlanSubListPropertyChanged -= DietPlanSubListPropertyChanged;

            DietPlanSubList.Remove(foodItem);

            SubTotalCalories -= foodItem.Calories;
        }

        private void DietPlanSubListPropertyChanged(object sender, EventArgs e)
        {
            int subTotalCalories = 0;

            foreach (var foodItem in DietPlanSubList)
            {
                subTotalCalories += foodItem.Calories;
            }

            SubTotalCalories = subTotalCalories;
        }
    }
}
