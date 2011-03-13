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

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class NutritionChartViewModel : NotificationObject, INutritionChartViewModel 
    {
        private readonly IEventAggregator _eventAggregator;
        private IList<NutritionChartItemViewModel> _nutritionChartItem;
        private string[] NutritionNames = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };

        public NutritionChartViewModel(INutritionChartView view, IEventAggregator eventAggregator)
        {
            NutritionChartItems = new List<NutritionChartItemViewModel>();
            
            for (int i = 0; i < 4; i++)
            {
                NutritionChartItemViewModel nutritionChartItem = new NutritionChartItemViewModel(NutritionNames[i]);
                NutritionChartItems.Add(nutritionChartItem);
            }
            
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<DietPlanNutritionChangeEvent>().Subscribe(DietPlanNutritionChangedHandler);
            _eventAggregator.GetEvent<FoodItemNutritionChangeEvent>().Subscribe(FoodItemNutritionChangedHandler);
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

        private void DietPlanNutritionChangedHandler(IList<NutritionalElement> nutritions)
        {
            for (int i = 0; i < 4; i++)
			{
                NutritionChartItems[i].DietPlanNutritionValue = nutritions[i].Value;
			}
        }

        private void FoodItemNutritionChangedHandler(IList<NutritionalElement> nutritions)
        {
            IList<NutritionalElement> Nutritions = nutritions;
            for (int i = 0; i < 4; i++)
            {
                if (nutritions[i].Name == NutritionNames[i])
                    NutritionChartItems[i].FoodItemNutritionValue = nutritions[i].Value;
            }
        }
    }
}
