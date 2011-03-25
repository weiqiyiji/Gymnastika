using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Services.Session;
using Microsoft.Practices.Unity;
using Gymnastika.Services.Models;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanNutritionChartViewModel : NotificationObject, IDietPlanNutritionChartViewModel
    {
        private string[] NutritionNames = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly User _user;
        private readonly int _beautyWeight;
        private readonly double _minTotalCalorie;
        private readonly double _maxTotalCalorie;
        private readonly double[] _nutritionMinMaxValues;
        private IList<DietPlanNutritionChartItemViewModel> _dietPlanNutritionChartItems;

        public DietPlanNutritionChartViewModel(
            IDietPlanNutritionChartView view,
            ISessionManager sessionManager,
            IEventAggregator eventAggregator)
        {
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;
            _beautyWeight = (_user.Height * _user.Height / 10000) * 19;
            _minTotalCalorie = _beautyWeight / 0.45 * 10;
            _maxTotalCalorie = _beautyWeight / 0.45 * 13;

            _nutritionMinMaxValues = new double[] { _minTotalCalorie, _maxTotalCalorie, _minTotalCalorie * 0.6 / 4, _maxTotalCalorie * 0.6 / 4, 
                _minTotalCalorie * 0.2 / 9, _maxTotalCalorie * 0.2 / 9, _minTotalCalorie * 0.2 / 4, _maxTotalCalorie * 0.2 / 4 };

            DietPlanNutritionChartItems = new List<DietPlanNutritionChartItemViewModel>();
            
            for (int i = 0; i < 4; i++)
            {
                var nutritionChartItem = new DietPlanNutritionChartItemViewModel();
                nutritionChartItem.NutritionName = NutritionNames[i];
                nutritionChartItem.MinTotalNutritionValue = _nutritionMinMaxValues[2 * i];
                nutritionChartItem.MaxTotalNutritionValue = _nutritionMinMaxValues[2 * i + 1];
                DietPlanNutritionChartItems.Add(nutritionChartItem);
            }
            
            View = view;
            View.Context = this;
            _eventAggregator.GetEvent<DietPlanNutritionChangedEvent>().Subscribe(DietPlanNutritionChangedHandler);
        }

        #region INutritionChartViewModel Members

        public IDietPlanNutritionChartView View { get; set; }

        public IList<DietPlanNutritionChartItemViewModel> DietPlanNutritionChartItems
        {
            get
            {
                return _dietPlanNutritionChartItems;
            }
            set
            {
                if (_dietPlanNutritionChartItems != value)
                {
                    _dietPlanNutritionChartItems = value;
                    RaisePropertyChanged("DietPlanNutritionChartItems");
                }
            }
        }

        public void DietPlanNutritionChangedHandler(IList<NutritionElement> nutritions)
        {
            for (int i = 0; i < 4; i++)
            {
                string nutritionName = NutritionNames.FirstOrDefault(n => n == DietPlanNutritionChartItems[i].NutritionName);

                if (!String.IsNullOrEmpty(nutritionName))
                {
                    foreach (var nutrition in nutritions)
                    {
                        if (nutrition.Name == nutritionName)
                        {
                            DietPlanNutritionChartItems[i].DietPlanNutritionValue = (double)nutrition.Value;
                        }
                    }
                }
                else
                {
                    DietPlanNutritionChartItems[i].DietPlanNutritionValue = 0;
                }
            }
        }

        #endregion
    }
}
