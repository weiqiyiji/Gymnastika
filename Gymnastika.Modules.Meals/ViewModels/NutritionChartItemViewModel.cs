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

        public NutritionChartItemViewModel()
        {
            PositionedFoodNutritionValue = 0.0;
            SelectedFoodNutritionValue = 0.0;
        }

        public string NutritionName { get; set; }

        public double TotalNutritionValue { get; set; }

        public double MaximunNutritionValue { get { return TotalNutritionValue / 2; } }

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
    }
}
