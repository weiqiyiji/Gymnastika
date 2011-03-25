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
        private readonly IEventAggregator _eventAggregator;
        private IList<NutritionChartItemViewModel> _nutritionChartItem;
        private readonly ISessionManager _sessionManager;
        private readonly User _user;
        private readonly int _beautyWeight;
        private readonly double _totalCalorie;
        private readonly double[] _nutritionMaxValues;

        public NutritionChartViewModel(INutritionChartView view,
            IEventAggregator eventAggregator,
            ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;
            _beautyWeight = (_user.Height * _user.Height / 10000) * 19;
            _totalCalorie = _beautyWeight / 0.45 * 13;

            _nutritionMaxValues = new double[] { _totalCalorie, _totalCalorie * 0.6 / 4, _totalCalorie * 0.2 / 9, _totalCalorie * 0.2 / 4 };

            NutritionChartItems = new List<NutritionChartItemViewModel>();
            
            for (int i = 0; i < 4; i++)
            {
                NutritionChartItemViewModel nutritionChartItem = new NutritionChartItemViewModel();
                nutritionChartItem.NutritionName = NutritionNames[i];
                nutritionChartItem.TotalNutritionValue = _nutritionMaxValues[i];
                NutritionChartItems.Add(nutritionChartItem);
            }
            
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PositionedFoodNutritionChangedEvent>().Subscribe(PositionedFoodNutritionChangedHandler);
            _eventAggregator.GetEvent<SelectedFoodNutritionChangedEvent>().Subscribe(SelectedFoodNutritionChangedHandler);
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

        private void PositionedFoodNutritionChangedHandler(IList<NutritionElement> nutritions)
        {
            for (int i = 0; i < 4; i++)
            {
                string nutritionName = NutritionNames.FirstOrDefault(n => n == NutritionChartItems[i].NutritionName);

                if (!String.IsNullOrEmpty(nutritionName))
                {
                    foreach (var nutrition in nutritions)
                    {
                        if (nutrition.Name == nutritionName)
                        {
                            NutritionChartItems[i].PositionedFoodName = nutrition.Food.Name;
                            NutritionChartItems[i].PositionedFoodNutritionValue = (double)nutrition.Value;
                        }
                    }
                }
                else
                {
                    NutritionChartItems[i].PositionedFoodNutritionValue = 0;
                }
            }
        }

        private void SelectedFoodNutritionChangedHandler(IList<NutritionElement> nutritions)
        {
            for (int i = 0; i < 4; i++)
            {
                string nutritionName = NutritionNames.FirstOrDefault(n => n == NutritionChartItems[i].NutritionName);

                if (!String.IsNullOrEmpty(nutritionName))
                {
                    foreach (var nutrition in nutritions)
                    {
                        if (nutrition.Name == nutritionName)
                        {
                            NutritionChartItems[i].SelectedFoodName = nutrition.Food.Name;
                            NutritionChartItems[i].SelectedFoodNutritionValue = (double)nutrition.Value;
                        }
                    }
                }
                else
                {
                    NutritionChartItems[i].SelectedFoodNutritionValue = 0;
                }
            }
        }
    }
}
