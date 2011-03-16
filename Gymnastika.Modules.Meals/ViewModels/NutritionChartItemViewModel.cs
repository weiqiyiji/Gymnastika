using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class NutritionChartItemViewModel : NotificationObject, INutritionChartItemViewModel
    {
        private const int RefreshInterval = 1;
        DispatcherTimer _dietPlanNutritionAnimationTimer;
        DispatcherTimer _foodItemNutritionAnimationTimer;
        private double _dietPlanNutritionValue;
        private double _foodItemNutritionValue;
        private double _minTotalNutritionValue;
        private double _maxTotalNutritionValue;
        private double _oldDietPlanNutritionValue;
        private double _oldFoodItemNutritionValue;
        private double _currentFoodItemNutritionWidth;
        private double _currentDietPlanNutritionWidth;

        public NutritionChartItemViewModel(INutritionChartItemView view)
        {
            DietPlanNutritionValue = 0.0;
            OldDietPlanNutritionValue = 0.0;
            FoodItemNutritionValue = 0.0;
            OldFoodItemNutritionValue = 0.0;
            CurrentDietPlanNutritionWidth = 0.0;
            CurrentFoodItemNutritionWidth = 0.0;
            View = view;
            View.Context = this;
        }

        #region INutritionChartItemViewModel Members

        public INutritionChartItemView View { get; set; }

        public string NutritionName { get; set; }

        public double MinTotalNutritionValue
        {
            get
            {
                return _minTotalNutritionValue;
            }
            set
            {
                if (_minTotalNutritionValue != value)
                {
                    _minTotalNutritionValue = value;
                    RaisePropertyChanged("MinTotalNutritionValue");
                }
            }
        }

        public double MaxTotalNutritionValue
        {
            get
            {
                return _maxTotalNutritionValue;
            }
            set
            {
                if (_maxTotalNutritionValue != value)
                {
                    _maxTotalNutritionValue = value;
                    RaisePropertyChanged("MaxTotalNutritionValue");
                }
            }
        }

        public double DietPlanNutritionValue
        {
            get
            {
                return _dietPlanNutritionValue;
            }
            set
            {
                if (_dietPlanNutritionValue != value)
                {
                    _dietPlanNutritionValue = value;
                    RaisePropertyChanged("DietPlanNutritionValue");
                }
            }
        }

        public double OldDietPlanNutritionValue
        {
            get
            {
                return _oldDietPlanNutritionValue;
            }
            set
            {
                if (_oldDietPlanNutritionValue != value)
                {
                    _oldDietPlanNutritionValue = value;
                    RaisePropertyChanged("OldDietPlanNutritionValue");
                }
            }
        }

        public double FoodItemNutritionValue
        {
            get
            {
                return _foodItemNutritionValue;
            }
            set
            {
                if (_foodItemNutritionValue != value)
                {
                    _foodItemNutritionValue = value;
                    RaisePropertyChanged("FoodItemNutritionValue");
                }
            }
        }

        public double OldFoodItemNutritionValue
        {
            get
            {
                return _oldFoodItemNutritionValue;
            }
            set
            {
                if (_oldFoodItemNutritionValue != value)
                {
                    _oldFoodItemNutritionValue = value;
                    RaisePropertyChanged("OldFoodItemNutritionValue");
                }
            }
        }

        public double OldDietPlanNutritionWidth
        {
            get
            {
                return (150 / MinTotalNutritionValue * OldDietPlanNutritionValue);
            }
        }

        public double NewDietPlanNutritionWidth
        {
            get
            {
                return (150 / MinTotalNutritionValue * DietPlanNutritionValue);
            }
        }

        public double CurrentDietPlanNutritionWidth
        {
            get
            {
                return _currentDietPlanNutritionWidth;
            }
            set
            {
                if (_currentDietPlanNutritionWidth != value)
                {
                    _currentDietPlanNutritionWidth = value;
                    RaisePropertyChanged("CurrentDietPlanNutritionWidth");
                }
            }
        }

        public double OldFoodItemNutritionWidth
        {
            get
            {
                return (150 / MinTotalNutritionValue * OldFoodItemNutritionValue);
            }
        }

        public double NewFoodItemNutritionWidth
        {
            get
            {
                return (150 / MinTotalNutritionValue * FoodItemNutritionValue);
            }
        }

        public double CurrentFoodItemNutritionWidth
        {
            get
            {
                return _currentFoodItemNutritionWidth;
            }
            set
            {
                if (_currentFoodItemNutritionWidth != value)
                {
                    _currentFoodItemNutritionWidth = value;
                    RaisePropertyChanged("CurrentFoodItemNutritionWidth");
                }
            }
        }

        public void BeginDietPlanAnimation()
        {
            _dietPlanNutritionAnimationTimer = new DispatcherTimer();
            _dietPlanNutritionAnimationTimer.Interval = TimeSpan.FromMilliseconds(RefreshInterval);
            _dietPlanNutritionAnimationTimer.Tick += DietPlanNutritionTimer_Tick;
            _dietPlanNutritionAnimationTimer.Start();
        }

        public void BeginFoodItemAnimation()
        {
            _foodItemNutritionAnimationTimer = new DispatcherTimer();
            _foodItemNutritionAnimationTimer.Interval = TimeSpan.FromMilliseconds(RefreshInterval);
            _foodItemNutritionAnimationTimer.Tick += FoodItemNutritionTimer_Tick;
            _foodItemNutritionAnimationTimer.Start();
        }

        #endregion

        private void DietPlanNutritionTimer_Tick(object sender, EventArgs e)
        {
            if (OldDietPlanNutritionWidth.CompareTo(NewDietPlanNutritionWidth) < 0)
            {
                double tempWidth = CurrentDietPlanNutritionWidth + 1.0;
                if (tempWidth > 250)
                {
                    CurrentDietPlanNutritionWidth = 250;
                    _dietPlanNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentDietPlanNutritionWidth = tempWidth;

                if (CurrentDietPlanNutritionWidth >= NewDietPlanNutritionWidth)
                    _dietPlanNutritionAnimationTimer.Stop();
            }
            else
            {
                double tempWidth = CurrentDietPlanNutritionWidth - 1.0;
                if (tempWidth < 0)
                {
                    CurrentDietPlanNutritionWidth = 0;
                    _dietPlanNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentDietPlanNutritionWidth = tempWidth;

                if (CurrentDietPlanNutritionWidth <= NewDietPlanNutritionWidth)
                    _dietPlanNutritionAnimationTimer.Stop();
            }
        }

        private void FoodItemNutritionTimer_Tick(object sender, EventArgs e)
        {
            if (OldFoodItemNutritionWidth.CompareTo(NewFoodItemNutritionWidth) < 0)
            {
                double tempWidth = CurrentFoodItemNutritionWidth + 1.0;
                if (tempWidth > 250)
                {
                    CurrentFoodItemNutritionWidth = 250;
                    _foodItemNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentFoodItemNutritionWidth = CurrentFoodItemNutritionWidth + 1.0;

                if (CurrentFoodItemNutritionWidth >= NewFoodItemNutritionWidth)
                    _foodItemNutritionAnimationTimer.Stop();
            }
            else
            {
                double tempWidth = CurrentFoodItemNutritionWidth - 1.0;
                if (tempWidth < 0)
                {
                    CurrentFoodItemNutritionWidth = 0;
                    _foodItemNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentFoodItemNutritionWidth = tempWidth;

                if (CurrentFoodItemNutritionWidth <= NewFoodItemNutritionWidth)
                    _foodItemNutritionAnimationTimer.Stop();
            }
        }
    }
}
