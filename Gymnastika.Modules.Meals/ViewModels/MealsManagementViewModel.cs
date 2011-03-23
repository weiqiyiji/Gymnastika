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
using Gymnastika.Data;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using Gymnastika.Services.Models;
using System.Windows;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class MealsManagementViewModel : NotificationObject, IMealsManagementViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _searchString;
        private DateTime _createdDate;
        private ICommand _saveCommand;
        private ICommand _searchCommand;

        public MealsManagementViewModel(
            IMealsManagementView view,
            ICategoryListViewModel categoryListViewModel,
            IDietPlanListViewModel dietPlanListViewModel,
            INutritionChartViewModel nutritionChartViewModel,
            IPositionedFoodViewModel positionedFoodViewModel,
            IDietPlanNutritionChartViewModel dietPlanNutritionChartViewModel,
            IFoodService foodService,
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager,
            IUnityContainer container,
            IEventAggregator eventAggregator)
        {
            CategoryListViewModel = categoryListViewModel;
            DietPlanListViewModel = dietPlanListViewModel;
            NutritionChartViewModel = nutritionChartViewModel;
            PositionedFoodViewModel = positionedFoodViewModel;
            DietPlanNutritionChartViewModel = dietPlanNutritionChartViewModel;
            CreatedDate = DateTime.Now;
            TotalCalories = DietPlanListViewModel.TotalCalories;
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            _container = container;
            _eventAggregator = eventAggregator;
            CurrentUser = _sessionManager.GetCurrentSession().AssociatedUser;
            //using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            //{
            //    InMemoryFoods = _foodService.FoodProvider.GetAll();
            //}
            View = view;
            View.Context = this;
            View.SearchKeyDown += new KeyEventHandler(SearchKeyDown);

            _eventAggregator.GetEvent<SelectDateEvent>().Subscribe(SelectDateEventHandler);
        }

        private void SelectDateEventHandler(DateTime dateTime)
        {
            CreatedDate = dateTime;
        }

        #region IMealsManagementViewModel Members

        public IMealsManagementView View { get; set; }

        public SelectDateView SelectDateView { get; set; }

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

        public DateTime CreatedDate
        {
            get
            {
                return _createdDate.Date;
            }
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    RaisePropertyChanged("CreatedDate");
                }
            }
        }

        public User CurrentUser { get; set; }

        public string UserName
        {
            get
            {
                return CurrentUser.UserName;
            }
            set
            {
                if (CurrentUser.UserName != value)
                {
                    CurrentUser.UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new DelegateCommand(Save);

                return _saveCommand;
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

        private ICommand _showMyFavoriteCommand;

        public ICommand ShowMyFavoriteCommand
        {
            get
            {
                if (_showMyFavoriteCommand == null)
                    _showMyFavoriteCommand = new DelegateCommand(ShowMyFavorite);

                return _showMyFavoriteCommand;
            }
        }

        private ICommand _selectDateCommand;

        public ICommand SelectDateCommand
        {
            get
            {
                if (_selectDateCommand == null)
                    _selectDateCommand = new DelegateCommand(ShowSelectDateView);

                return _selectDateCommand;
            }
        }

        private void ShowSelectDateView()
        {
            SelectDateView = _container.Resolve<SelectDateView>();
            SelectDateView.Owner = Application.Current.MainWindow;
            SelectDateView.ShowDialog();
        }

        public IEnumerable<Food> InMemoryFoods { get; set; }

        public ICollection<Food> SearchResults { get; set; }

        public ICategoryListViewModel CategoryListViewModel { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }

        private IDietPlanListViewModel _dietPlanListViewModel;
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

        private decimal _totalCalories;
        public decimal TotalCalories
        {
            get
            {
                return _totalCalories;
            }
            set
            {
                if (_totalCalories != value)
                {
                    _totalCalories = value;
                    RaisePropertyChanged("TotalCalories");
                }
            }
        }

        public IDietPlanNutritionChartViewModel DietPlanNutritionChartViewModel { get; set; }

        public INutritionChartViewModel NutritionChartViewModel { get; set; }

        public IPositionedFoodViewModel PositionedFoodViewModel { get; set; }

        #endregion

        //private bool ValidateExistSavedDietPlans()
        //{
        //    int userId = _sessionManager.GetCurrentSession().AssociatedUser.Id;

        //    IEnumerable<DietPlan> savedDietPlans;
        //    using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
        //    {
        //        savedDietPlans = _foodService.DietPlanProvider.GetDietPlans(userId);
        //    }

        //    if (savedDietPlans.Count() != 0) return true;

        //    return false;
        //}

        private void ShowMyFavorite()
        {
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                FoodListViewModel.FavoriteFood = _foodService.FavoriteFoodProvider.Get(CurrentUser.Id);
                if (FoodListViewModel.FavoriteFood != null)
                    FoodListViewModel.FavoriteFood.Foods = _foodService.FoodProvider.GetFoods(FoodListViewModel.FavoriteFood).ToList();
            }

            FoodListViewModel.ShowMyFavoriteResult();
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

            using (var scope = _workEnvironment.GetWorkContextScope())
            {
                FoodListViewModel.SearchFoods = _foodService.FoodProvider.GetFoods(SearchString);
            }

            FoodListViewModel.ShowSearchResult();
        }

        private void Save()
        {
            DietPlan dietPlan = new DietPlan();
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                dietPlan.User = _sessionManager.GetCurrentSession().AssociatedUser;
                dietPlan.PlanType = PlanType.CreatedDietPlan;
                dietPlan.CreatedDate = CreatedDate;
                dietPlan.SubDietPlans = new List<SubDietPlan>();
                _foodService.DietPlanProvider.Create(dietPlan);
                for (int i = 0; i < 3; i++)
                {
                    SubDietPlan subDietPlan = new SubDietPlan();
                    //subDietPlan.DietPlan = dietPlan;
                    subDietPlan.DietPlanItems = new List<DietPlanItem>();
                    _foodService.SubDietPlanProvider.Create(subDietPlan);
                    foreach (var foodItem in DietPlanListViewModel.DietPlanList[i].FoodItems)
                    {
                        DietPlanItem dietPlanItem = new DietPlanItem();
                        dietPlanItem.Food = foodItem.Food;
                        dietPlanItem.Amount = foodItem.Amount;
                        //dietPlanItem.SubDietPlan = subDietPlan;
                        _foodService.DietPlanItemProvider.Create(dietPlanItem);
                        subDietPlan.DietPlanItems.Add(dietPlanItem);
                        _foodService.DietPlanItemProvider.Update(dietPlanItem);
                    }
                    dietPlan.SubDietPlans.Add(subDietPlan);
                    _foodService.SubDietPlanProvider.Update(subDietPlan);
                }
            }
            System.Windows.MessageBox.Show("已保存");

            _eventAggregator.GetEvent<NotifyHistoryDietPlanChangedEvent>().Publish(dietPlan);
        }
    }
}
