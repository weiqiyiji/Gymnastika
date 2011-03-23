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
    public class NutritionChartItemViewModel : NotificationObject
    {
        private const double RefreshInterval = 0.2;
        private string _firstFoodName;
        private string _secondFoodName;
        DispatcherTimer _firstFoodNutritionAnimationTimer;
        DispatcherTimer _secondFoodNutritionAnimationTimer;
        private double _newFirstFoodNutritionValue;
        private double _newSecondFoodNutritionValue;
        private double _minTotalNutritionValue;
        private double _maxTotalNutritionValue;
        private double _oldDietPlanNutritionValue;
        private double _oldSecondFoodNutritionValue;
        private double _currentSecondFoodNutritionWidth;
        private double _currentDietPlanNutritionWidth;

        public NutritionChartItemViewModel()
        {
            NewFirstFoodNutritionValue = 0.0;
            OldDietPlanNutritionValue = 0.0;
            NewSecondFoodNutritionValue = 0.0;
            OldSecondFoodNutritionValue = 0.0;
            CurrentFirstFoodNutritionWidth = 0.0;
            CurrentSecondFoodNutritionWidth = 0.0;
        }

        public string NutritionName { get; set; }

        public string FirstFoodName
        {
            get
            {
                return _firstFoodName;
            }
            set
            {
                if (_firstFoodName != value)
                {
                    _firstFoodName = value;
                    RaisePropertyChanged("FirstFoodName");
                }
            }
        }

        public string SecondFoodName
        {
            get
            {
                return _secondFoodName;
            }
            set
            {
                if (_secondFoodName != value)
                {
                    _secondFoodName = value;
                    RaisePropertyChanged("SecondFoodName");
                }
            }
        }

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

        public double NewFirstFoodNutritionValue
        {
            get
            {
                return _newFirstFoodNutritionValue;
            }
            set
            {
                if (_newFirstFoodNutritionValue != value)
                {
                    _newFirstFoodNutritionValue = value;
                    RaisePropertyChanged("NewFirstFoodNutritionValue");
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

        public double NewSecondFoodNutritionValue
        {
            get
            {
                return _newSecondFoodNutritionValue;
            }
            set
            {
                if (_newSecondFoodNutritionValue != value)
                {
                    _newSecondFoodNutritionValue = value;
                    RaisePropertyChanged("NewSecondFoodNutritionValue");
                }
            }
        }

        public double OldSecondFoodNutritionValue
        {
            get
            {
                return _oldSecondFoodNutritionValue;
            }
            set
            {
                if (_oldSecondFoodNutritionValue != value)
                {
                    _oldSecondFoodNutritionValue = value;
                    RaisePropertyChanged("OldSecondFoodNutritionValue");
                }
            }
        }

        public double OldFirstFoodNutritionWidth
        {
            get
            {
                return (500 / MinTotalNutritionValue * OldDietPlanNutritionValue);
            }
        }

        public double NewFirstFoodNutritionWidth
        {
            get
            {
                return (500 / MinTotalNutritionValue * NewFirstFoodNutritionValue);
            }
        }

        public double CurrentFirstFoodNutritionWidth
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
                    RaisePropertyChanged("CurrentFirstFoodNutritionWidth");
                }
            }
        }

        public double OldSecondFoodNutritionWidth
        {
            get
            {
                return (500 / MinTotalNutritionValue * OldSecondFoodNutritionValue);
            }
        }

        public double NewSecondFoodNutritionWidth
        {
            get
            {
                return (500 / MinTotalNutritionValue * NewSecondFoodNutritionValue);
            }
        }

        public double CurrentSecondFoodNutritionWidth
        {
            get
            {
                return _currentSecondFoodNutritionWidth;
            }
            set
            {
                if (_currentSecondFoodNutritionWidth != value)
                {
                    _currentSecondFoodNutritionWidth = value;
                    RaisePropertyChanged("CurrentSecondFoodNutritionWidth");
                }
            }
        }

        public void BeginFirstFoodAnimation()
        {
            _firstFoodNutritionAnimationTimer = new DispatcherTimer();
            _firstFoodNutritionAnimationTimer.Interval = TimeSpan.FromMilliseconds(RefreshInterval);
            _firstFoodNutritionAnimationTimer.Tick += FirstFoodNutritionTimer_Tick;
            _firstFoodNutritionAnimationTimer.Start();
        }

        public void BeginSecondFoodAnimation()
        {
            _secondFoodNutritionAnimationTimer = new DispatcherTimer();
            _secondFoodNutritionAnimationTimer.Interval = TimeSpan.FromMilliseconds(RefreshInterval);
            _secondFoodNutritionAnimationTimer.Tick += SecondFoodNutritionTimer_Tick;
            _secondFoodNutritionAnimationTimer.Start();
        }

        private void FirstFoodNutritionTimer_Tick(object sender, EventArgs e)
        {
            if (OldFirstFoodNutritionWidth.CompareTo(NewFirstFoodNutritionWidth) < 0)
            {
                double tempWidth = CurrentFirstFoodNutritionWidth + 1.0;
                if (tempWidth > 250)
                {
                    CurrentFirstFoodNutritionWidth = 250;
                    _firstFoodNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentFirstFoodNutritionWidth = tempWidth;

                if (CurrentFirstFoodNutritionWidth >= NewFirstFoodNutritionWidth)
                    _firstFoodNutritionAnimationTimer.Stop();
            }
            else
            {
                double tempWidth = CurrentFirstFoodNutritionWidth - 1.0;
                if (tempWidth < 0)
                {
                    CurrentFirstFoodNutritionWidth = 0;
                    _firstFoodNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentFirstFoodNutritionWidth = tempWidth;

                if (CurrentFirstFoodNutritionWidth <= NewFirstFoodNutritionWidth)
                    _firstFoodNutritionAnimationTimer.Stop();
            }
        }

        private void SecondFoodNutritionTimer_Tick(object sender, EventArgs e)
        {
            if (OldSecondFoodNutritionWidth.CompareTo(NewSecondFoodNutritionWidth) < 0)
            {
                double tempWidth = CurrentSecondFoodNutritionWidth + 1.0;
                if (tempWidth > 250)
                {
                    CurrentSecondFoodNutritionWidth = 250;
                    _secondFoodNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentSecondFoodNutritionWidth = CurrentSecondFoodNutritionWidth + 1.0;

                if (CurrentSecondFoodNutritionWidth >= NewSecondFoodNutritionWidth)
                    _secondFoodNutritionAnimationTimer.Stop();
            }
            else
            {
                double tempWidth = CurrentSecondFoodNutritionWidth - 1.0;
                if (tempWidth < 0)
                {
                    CurrentSecondFoodNutritionWidth = 0;
                    _secondFoodNutritionAnimationTimer.Stop();
                    return;
                }

                CurrentSecondFoodNutritionWidth = tempWidth;

                if (CurrentSecondFoodNutritionWidth <= NewSecondFoodNutritionWidth)
                    _secondFoodNutritionAnimationTimer.Stop();
            }
        }
    }
}
