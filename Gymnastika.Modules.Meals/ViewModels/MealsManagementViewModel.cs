using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System.Windows.Data;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class MealsManagementViewModel : NotificationObject, IMealsManagementViewModel
    {
        private string _searchString;
        private ICommand _searchCommand;
        private ICommand _recommendDietPlanCommand;

        public MealsManagementViewModel(IMealsManagementView view, IFoodListViewModel foodListViewModel, ICreateDietPlanViewModel createDietPlanViewModel, IRecommendDietPlanViewModel recommendDietPlanViewModel)
        {
            FoodListViewModel = foodListViewModel;
            CreateDietPlanViewModel = createDietPlanViewModel;
            RecommendDietPlanViewModel = recommendDietPlanViewModel;
            recommendDietPlanViewModel.Apply += new EventHandler(Apply);
            View = view;
            View.Context = this;
            View.SearchKeyDown += new KeyEventHandler(SearchKeyDown);
        }

        #region IMealsManagementViewModel Members

        public IMealsManagementView View { get; set; }

        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                if (_searchString != value)
                {
                    _searchString = value;
                    RaisePropertyChanged("SearchString");
                }
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new DelegateCommand(Search);

                return _searchCommand;
            }
        }

        public ICommand ShowRecommendedDietPlanCommand
        {
            get
            {
                if (_recommendDietPlanCommand == null)
                    _recommendDietPlanCommand = new DelegateCommand(ShowRecommendedDietPlan);

                return _recommendDietPlanCommand;
            }
        }

        public IEnumerable<Food> InMemoryFoods { get; set; }

        public IEnumerable<Food> SearchResults { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }

        public ICreateDietPlanViewModel CreateDietPlanViewModel { get; set; }

        public IRecommendDietPlanViewModel RecommendDietPlanViewModel { get; set; }

        #endregion

        private void Apply(object sender, EventArgs e)
        {
            CreateDietPlanViewModel.DietPlanListViewModel = RecommendDietPlanViewModel.DietPlanListViewModel;
        }

        private void SearchKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Search();
        }

        private void Search()
        {
            BindingExpression binding = View.GetBindingSearchString();
            binding.UpdateSource();

            //SearchResults = ...
            FoodListViewModel.InMemoryFoodList = (IEnumerable<FoodItemViewModel>)SearchResults;
            FoodListViewModel.Initialize();
        }

        private void ShowRecommendedDietPlan()
        {
            RecommendDietPlanViewModel.View.ShowView();
            RecommendDietPlanViewModel.DietPlanListViewModel.View.ExpandAll();
        }
    }
}
