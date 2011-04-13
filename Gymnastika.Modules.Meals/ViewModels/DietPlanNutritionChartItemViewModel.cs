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
        private double _dietPlanNutritionValue;
        private double _minTotalNutritionValue;
        private double _maxTotalNutritionValue;

        public DietPlanNutritionChartItemViewModel()
        {
            DietPlanNutritionValue = 0.0;
            NeedAlert = true;
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

        public bool IsOverMaxValue
        {
            get
            {
                return (DietPlanNutritionValue >= MaxTotalNutritionValue);
            }
        }

        public bool NeedAlert { get; set; }
    }
}
