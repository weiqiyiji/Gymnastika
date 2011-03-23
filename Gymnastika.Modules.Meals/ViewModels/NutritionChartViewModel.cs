using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class NutritionChartViewModel : NotificationObject, INutritionChartViewModel
    {
        private string[] NutritionNames = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly User _user;
        private readonly int _beautyWeight;
        private readonly double _minTotalCalorie;
        private readonly double _maxTotalCalorie;
        private readonly double[] _nutritionMinMaxValues;
        private IList<NutritionChartItemViewModel> _nutritionChartItem;

        public NutritionChartViewModel(INutritionChartView view,
            IEventAggregator eventAggregator,
            ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;
            _beautyWeight = (_user.Height * _user.Height / 10000) * 19;
            _minTotalCalorie = _beautyWeight / 0.45 * 10;
            _maxTotalCalorie = _beautyWeight / 0.45 * 13;

            _nutritionMinMaxValues = new double[] { _minTotalCalorie, _maxTotalCalorie, _minTotalCalorie * 0.7 / 4, _maxTotalCalorie * 0.7 / 4, 
                _minTotalCalorie * 0.15 / 9, _maxTotalCalorie * 0.15 / 9, _minTotalCalorie * 0.15 / 4, _maxTotalCalorie * 0.15 / 4 };

            NutritionChartItems = new List<NutritionChartItemViewModel>();
            
            for (int i = 0; i < 4; i++)
            {
                NutritionChartItemViewModel nutritionChartItem = new NutritionChartItemViewModel();
                nutritionChartItem.NutritionName = NutritionNames[i];
                nutritionChartItem.MinTotalNutritionValue = _nutritionMinMaxValues[2 * i];
                nutritionChartItem.MaxTotalNutritionValue = _nutritionMinMaxValues[2 * i + 1];
                NutritionChartItems.Add(nutritionChartItem);
            }
            
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PositionedFoodNutritionChangedEvent>().Subscribe(FirstFoodNutritionChangedHandler);
            _eventAggregator.GetEvent<SelectedFoodNutritionChangedEvent>().Subscribe(SecondFoodNutritionChangedHandler);
            View = view;
            View.Context = this;
        }

        #region INutritionChartViewModel Members

        public INutritionChartView View { get; set; }

        public IList<NutritionChartItemViewModel> NutritionChartItems
        {
            get
            {
                return _nutritionChartItem;
            }
            set
            {
                if (_nutritionChartItem != value)
                {
                    _nutritionChartItem = value;
                    RaisePropertyChanged("NutritionChartItems");
                }
            }
        }

        #endregion

        private void FirstFoodNutritionChangedHandler(IList<NutritionalElement> nutritions)
        {
            for (int i = 0; i < 4; i++)
            {
                NutritionChartItems[i].OldDietPlanNutritionValue = NutritionChartItems[i].NewFirstFoodNutritionValue;

                string nutritionName = NutritionNames.FirstOrDefault(n => n == NutritionChartItems[i].NutritionName);

                if (!String.IsNullOrEmpty(nutritionName))
                {
                    foreach (var nutrition in nutritions)
                    {
                        if (nutrition.Name == nutritionName)
                        {
                            NutritionChartItems[i].FirstFoodName = nutrition.Food.Name;
                            NutritionChartItems[i].NewFirstFoodNutritionValue = (double)nutrition.Value;
                        }
                    }
                }
                else
                {
                    NutritionChartItems[i].NewFirstFoodNutritionValue = 0;
                }
                
                NutritionChartItems[i].BeginFirstFoodAnimation();
            }
        }

        private void SecondFoodNutritionChangedHandler(IList<NutritionalElement> nutritions)
        {
            for (int i = 0; i < 4; i++)
            {
                NutritionChartItems[i].OldSecondFoodNutritionValue = NutritionChartItems[i].NewSecondFoodNutritionValue;

                string nutritionName = NutritionNames.FirstOrDefault(n => n == NutritionChartItems[i].NutritionName);

                if (!String.IsNullOrEmpty(nutritionName))
                {
                    foreach (var nutrition in nutritions)
                    {
                        if (nutrition.Name == nutritionName)
                        {
                            NutritionChartItems[i].SecondFoodName = nutrition.Food.Name;
                            NutritionChartItems[i].NewSecondFoodNutritionValue = (double)nutrition.Value;
                        }
                    }
                }
                else
                {
                    NutritionChartItems[i].NewSecondFoodNutritionValue = 0;
                }

                NutritionChartItems[i].BeginSecondFoodAnimation();
            }
        }
    }
}
