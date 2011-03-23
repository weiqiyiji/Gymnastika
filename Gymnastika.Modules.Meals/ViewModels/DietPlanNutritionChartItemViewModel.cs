using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanNutritionChartItemViewModel : NotificationObject
    {
        private const double RefreshInterval = 0.2;
        DispatcherTimer _dietPlanNutritionAnimationTimer;
        private double _dietPlanNutritionValue;
        private double _minTotalNutritionValue;
        private double _maxTotalNutritionValue;
        private double _oldDietPlanNutritionValue;
        private double _currentDietPlanNutritionWidth;

        public DietPlanNutritionChartItemViewModel()
        {
            DietPlanNutritionValue = 0.0;
            OldDietPlanNutritionValue = 0.0;
            CurrentDietPlanNutritionWidth = 0.0;
        }

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

        public void BeginDietPlanAnimation()
        {
            _dietPlanNutritionAnimationTimer = new DispatcherTimer();
            _dietPlanNutritionAnimationTimer.Interval = TimeSpan.FromMilliseconds(RefreshInterval);
            _dietPlanNutritionAnimationTimer.Tick += DietPlanNutritionTimer_Tick;
            _dietPlanNutritionAnimationTimer.Start();
        }

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
    }
}
