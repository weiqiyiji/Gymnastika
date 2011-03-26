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
        private string _positionedFoodName;
        private string _selectedFoodName;
        private double _positionedFoodNutritionValue;
        private double _selectedFoodNutritionValue;
        private double _maximunNutritionValue;

        public NutritionChartItemViewModel(string nutritionName, double totalNutritionValue)
        {
            NutritionName = nutritionName;
            TotalNutritionValue = totalNutritionValue;
            MaximunNutritionValue = TotalNutritionValue / 2;
            PositionedFoodNutritionValue = 0.0;
            SelectedFoodNutritionValue = 0.0;
        }

        public string NutritionName { get; set; }

        public double TotalNutritionValue { get; set; }

        public double MaximunNutritionValue
        {
            get
            {
                return _maximunNutritionValue;
            }
            set
            {
                if (_maximunNutritionValue != value)
                {
                    _maximunNutritionValue = value;
                    RaisePropertyChanged("MaximunNutritionValue");
                }
            }
        }

        public string PositionedFoodName
        {
            get
            {
                return _positionedFoodName;
            }
            set
            {
                if (_positionedFoodName != value)
                {
                    _positionedFoodName = value;
                    RaisePropertyChanged("PositionedFoodName");
                }
            }
        }

        public string SelectedFoodName
        {
            get
            {
                return _selectedFoodName;
            }
            set
            {
                if (_selectedFoodName != value)
                {
                    _selectedFoodName = value;
                    RaisePropertyChanged("SelectedFoodName");
                }
            }
        }
        public double PositionedFoodNutritionValue
        {
            get
            {
                return _positionedFoodNutritionValue;
            }
            set
            {
                if (_positionedFoodNutritionValue != value)
                {
                    _positionedFoodNutritionValue = value;
                    RaisePropertyChanged("PositionedFoodNutritionValue");
                }
            }
        }

        public double SelectedFoodNutritionValue
        {
            get
            {
                return _selectedFoodNutritionValue;
            }
            set
            {
                if (_selectedFoodNutritionValue != value)
                {
                    _selectedFoodNutritionValue = value;
                    RaisePropertyChanged("SelectedFoodNutritionValue");
                }
            }
        }

        public void Refresh()
        {
            MaximunNutritionValue = MaxOfThree(MaximunNutritionValue, PositionedFoodNutritionValue, SelectedFoodNutritionValue);
        }

        private double MaxOfThree(double one, double two, double three)
        {
            if (one >= two)
                return one >= three ? one : three;
            else
                return two >= three ? two : three;
        }
    }
}
