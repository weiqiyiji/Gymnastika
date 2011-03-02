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
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Services.Session;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class MealsManagementViewModel : NotificationObject, IMealsManagementViewModel
    {
        private readonly IFoodService _foodService;
        private string _searchString;
        private ICommand _searchCommand;
        private ICommand _showSavedDietPlanCommand;
        private ICommand _showRecommendDietPlanCommand;
        private readonly XDataHelpers.XDataRepository _db;

        public MealsManagementViewModel(
            IMealsManagementView view,
            IFoodListViewModel foodListViewModel,
            ICreateDietPlanViewModel createDietPlanViewModel,
            ISelectDietPlanViewModel recommendedDietPlanViewModel,
            ISelectDietPlanViewModel savedDietPlanViewModel,
            IFoodService foodService)
        {
            FoodListViewModel = foodListViewModel;
            CreateDietPlanViewModel = createDietPlanViewModel;
            InitializeSelectDietPlanViewModel(SavedDietPlanViewModel, savedDietPlanViewModel, PlanType.CreatedDietPlan, ApplySavedDietPlan);
            InitializeSelectDietPlanViewModel(RecommendedDietPlanViewModel, recommendedDietPlanViewModel, PlanType.RecommendedDietPlan, ApplyRecommendedDietPlan);
            _foodService = foodService;
            _db = new XDataHelpers.XDataRepository();
            InMemoryFoods = _db.Foods;
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

        public ICommand ShowSavedDietPlanCommand
        {
            get
            {
                if (_showSavedDietPlanCommand == null)
                    _showSavedDietPlanCommand = new DelegateCommand(ShowSavedDietPlan);

                return _showSavedDietPlanCommand;
            }
        }

        public ICommand ShowRecommendedDietPlanCommand
        {
            get
            {
                if (_showRecommendDietPlanCommand == null)
                    _showRecommendDietPlanCommand = new DelegateCommand(ShowRecommendedDietPlan);

                return _showRecommendDietPlanCommand;
            }
        }

        public IEnumerable<Food> InMemoryFoods { get; set; }

        public ICollection<Food> SearchResults { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }

        public ICreateDietPlanViewModel CreateDietPlanViewModel { get; set; }

        public ISelectDietPlanViewModel SavedDietPlanViewModel { get; set; }

        public ISelectDietPlanViewModel RecommendedDietPlanViewModel { get; set; }

        #endregion

        private void InitializeSelectDietPlanViewModel(
            ISelectDietPlanViewModel destViewModel,
            ISelectDietPlanViewModel srcViewModel,
            PlanType planType,
            EventHandler target)
        {
            destViewModel = srcViewModel;
            destViewModel.PlanType = planType;
            destViewModel.Apply += target;
            destViewModel.Initialize();
        }

        private void ApplySavedDietPlan(object sender, EventArgs e)
        {
            CreateDietPlanViewModel.DietPlanListViewModel = SavedDietPlanViewModel.DietPlanListViewModel;
        }

        private void ApplyRecommendedDietPlan(object sender, EventArgs e)
        {
            CreateDietPlanViewModel.DietPlanListViewModel = RecommendedDietPlanViewModel.DietPlanListViewModel;
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

            SearchResults = new Collection<Food>();
            foreach (var food in InMemoryFoods)
            {
                string filter = SearchString.ToUpper(CultureInfo.InvariantCulture);
                if (food.Name.ToUpper(CultureInfo.InvariantCulture).Contains(filter))
                    SearchResults.Add(food);
            }

            FoodListViewModel.CurrentFoods = SearchResults;
            FoodListViewModel.Initialize();
        }

        private void ShowSavedDietPlan()
        {
            ShowSelectDietPlan(SavedDietPlanViewModel);
        }

        private void ShowRecommendedDietPlan()
        {
            ShowSelectDietPlan(RecommendedDietPlanViewModel);
        }

        private void ShowSelectDietPlan(ISelectDietPlanViewModel selectDietPlanViewModel)
        {
            selectDietPlanViewModel.View.ShowView();
            selectDietPlanViewModel.DietPlanListViewModel.View.ExpandAll();
        }
    }
}
