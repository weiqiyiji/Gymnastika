using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Modules.Meals.Services;
using Microsoft.Practices.ServiceLocation;
using System.Collections.ObjectModel;
using Gymnastika.Services.Session;
using Gymnastika.Data;
using Gymnastika.Services.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class SelectDietPlanViewModel : NotificationObject, ISelectDietPlanViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private int _currentPage;
        private int _pageCount;
        private ICommand _showPreviousPageCommand;
        private ICommand _showNextPageCommand;
        private ICommand _applyCommand;
        private IDietPlanListViewModel _dietPlanLlistViewModel;

        public SelectDietPlanViewModel(
            ISelectDietPlanView view, 
            IFoodService foodService,
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            View = view;
            View.Context = this;
        }

        #region IRecommendDietPlanViewModel Members

        public ISelectDietPlanView View { get; set; }

        public IDietPlanListViewModel DietPlanListViewModel
        {
            get
            {
                return _dietPlanLlistViewModel;
            }
            set
            {
                if (_dietPlanLlistViewModel != value)
                {
                    _dietPlanLlistViewModel = value;
                    RaisePropertyChanged("DietPlanListViewModel");
                }
            }
        }

        public DietPlan CurrentDietPlan { get; set; }

        public IList<DietPlan> InMemoryDietPlans { get; set; }

        public User CurrentUser { get; set; }

        public PlanType PlanType { get; set; }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    RaisePropertyChanged("CurrentPage");
                }
            }
        }

        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                if (_pageCount != value)
                {
                    _pageCount = value;
                    RaisePropertyChanged("PageCount");
                }
            }
        }

        public ICommand ShowPreviousPageCommand
        {
            get
            {
                if (_showPreviousPageCommand == null)
                    _showPreviousPageCommand = new DelegateCommand(ShowPreviousPage);

                return _showPreviousPageCommand;
            }
        }

        public ICommand ShowNextPageCommand
        {
            get
            {
                if (_showNextPageCommand == null)
                    _showNextPageCommand = new DelegateCommand(ShowNextPage);

                return _showNextPageCommand;
            }
        }

        public ICommand ApplyCommand
        {
            get
            {
                if (_applyCommand == null)
                    _applyCommand = new DelegateCommand(OnApply);

                return _applyCommand;
            }
        }

        public event EventHandler Apply;

        public void Initialize()
        {
            CurrentPage = 1;

            switch (PlanType)
            {
                case PlanType.CreatedDietPlan:
                    CurrentUser = _sessionManager.GetCurrentSession().AssociatedUser;
                    using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
                    {
                        //InMemoryDietPlans = _foodService.DietPlanProvider.GetDietPlans(userId).ToList();
                        //LoadDietPlans();
                        PageCount = _foodService.DietPlanProvider.count(CurrentUser);
                        CurrentDietPlan = _foodService.DietPlanProvider.Get(CurrentUser, CurrentPage - 1);
                        LoadDietPlan();
                    }
                    break;
                case PlanType.RecommendedDietPlan:
                    using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
                    {
                        //InMemoryDietPlans = _foodService.DietPlanProvider.GetRecommendedDietPlans().ToList();
                        PageCount = _foodService.DietPlanProvider.count(PlanType);
                        CurrentDietPlan = _foodService.DietPlanProvider.Get(PlanType, CurrentPage - 1);
                        LoadDietPlan();
                    }
                    break;
                default:
                    break;
            }

            //PageCount = InMemoryDietPlans.Count;

            InitializeDietPlanList();
        }

        #endregion

        private void LoadDietPlan()
        {
            //foreach (var CurrentDietPlan in InMemoryDietPlans)
            //{
            //    CurrentDietPlan.SubDietPlans = _foodService.SubDietPlanProvider.GetSubDietPlans(CurrentDietPlan).ToList();

            //    foreach (var subDietPlan in CurrentDietPlan.SubDietPlans)
            //    {
            //        subDietPlan.DietPlanItems = _foodService.DietPlanItemProvider.GetDietPlanItems(subDietPlan).ToList();

            //        foreach (var CurrentDietPlanItem in subDietPlan.DietPlanItems)
            //        {
            //            CurrentDietPlanItem.Food = _foodService.FoodProvider.Get(CurrentDietPlanItem);
            //        }
            //    }
            //}
            CurrentDietPlan.SubDietPlans = _foodService.SubDietPlanProvider.GetSubDietPlans(CurrentDietPlan).ToList();

            foreach (var subDietPlan in CurrentDietPlan.SubDietPlans)
            {
                subDietPlan.DietPlanItems = _foodService.DietPlanItemProvider.GetDietPlanItems(subDietPlan).ToList();

                foreach (var CurrentDietPlanItem in subDietPlan.DietPlanItems)
                {
                    CurrentDietPlanItem.Food = _foodService.FoodProvider.Get(CurrentDietPlanItem);
                }
            }
        }

        private void InitializeDietPlanList()
        {
            DietPlanListViewModel = ServiceLocator.Current.GetInstance<IDietPlanListViewModel>();

            for (int i = 0; i < 6; i++)
            {
                //foreach (var dietPlanItem in InMemoryDietPlans[CurrentPage - 1].SubDietPlans[i].DietPlanItems)
                //{
                //    DietPlanListViewModel.DietPlanList[i].AddFoodToPlan(new FoodItemViewModel(dietPlanItem.Food)
                //    {
                //        Amount = dietPlanItem.Amount
                //    });
                //}
                foreach (var dietPlanItem in CurrentDietPlan.SubDietPlans[i].DietPlanItems)
                {
                    DietPlanListViewModel.DietPlanList[i].AddFoodToPlan(new FoodItemViewModel(dietPlanItem.Food)
                    {
                        Amount = dietPlanItem.Amount
                    });
                }
            }
        }

        private void SwitchPlanType()
        {
            switch (PlanType)
            {
                case PlanType.CreatedDietPlan:
                    using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
                    {
                        CurrentDietPlan = _foodService.DietPlanProvider.Get(CurrentUser, CurrentPage - 1);
                        LoadDietPlan();
                    }
                    break;
                case PlanType.RecommendedDietPlan:
                    using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
                    {
                        CurrentDietPlan = _foodService.DietPlanProvider.Get(PlanType, CurrentPage - 1);
                        LoadDietPlan();
                    }
                    break;
                default:
                    break;
            }
        }
        
        private void ShowPreviousPage()
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;

            SwitchPlanType();

            InitializeDietPlanList();
        }

        private void ShowNextPage()
        {
            if (CurrentPage >= PageCount) return;

            CurrentPage++;

            SwitchPlanType();

            InitializeDietPlanList();
        }

        private void OnApply()
        {
            if (Apply != null)
                Apply(this, new EventArgs());
        }
    }
}
