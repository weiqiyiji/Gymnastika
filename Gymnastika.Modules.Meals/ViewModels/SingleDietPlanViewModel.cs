using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class SingleDietPlanViewModel : NotificationObject, ISingleDietPlanViewModel
    {
        private IDietPlanListViewModel _dietPlanListViewModel;

        public SingleDietPlanViewModel(ISingleDietPlanView view, 
            IDietPlanListViewModel dietPlanListViewModel,
            IDietPlanNutritionChartViewModel dietPlanNutritionChartViewModel)
        {
            DietPlanListViewModel = dietPlanListViewModel;
            DietPlanListViewModel.DietPlanNutritionChanged += new EventHandler(DietPlanNutritionChanged);
            DietPlanNutritionChartViewModel = dietPlanNutritionChartViewModel;
            View = view;
            View.Context = this;
        }

        #region ISingleDietPlanViewModel Members

        public ISingleDietPlanView View { get; set; }

        public IDietPlanListViewModel DietPlanListViewModel
        {
            get
            {
                return _dietPlanListViewModel;
            }
            set
            {
                if (_dietPlanListViewModel != value)
                {
                    _dietPlanListViewModel = value;
                    RaisePropertyChanged("DietPlanListViewModel");
                }
            }
        }

        public IDietPlanNutritionChartViewModel DietPlanNutritionChartViewModel { get; set; }

        #endregion

        private void DietPlanNutritionChanged(object sender, EventArgs e)
        {
            IDietPlanListViewModel dietPlanListViewModel = sender as IDietPlanListViewModel;

            DietPlanNutritionChartViewModel.DietPlanNutritionChangedHandler(dietPlanListViewModel.Nutritions);
        }
    }
}
