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
using Gymnastika.Services.Session;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class SelectDietPlanViewModel : NotificationObject, ISelectDietPlanViewModel
    {
        private readonly IFoodService _foodService;
        private readonly ISessionManager _sessionManager;
        private int _currentPage;
        private int _pageCount;
        private ICommand _showPreviousPageCommand;
        private ICommand _showNextPageCommand;
        private ICommand _applyCommand;

        public SelectDietPlanViewModel(
            ISelectDietPlanView view, 
            IDietPlanListViewModel dietPlanListViewModel,
            //IFoodService foodService,
            ISessionManager sessionManager)
        {
            DietPlanListViewModel = dietPlanListViewModel;
            //_foodService = foodService;
            _sessionManager = sessionManager;
            View = view;
            View.Context = this;
            Initialize();
        }

        #region IRecommendDietPlanViewModel Members

        public ISelectDietPlanView View { get; set; }

        public IDietPlanListViewModel DietPlanListViewModel { get; set; }

        public IList<DietPlan> InMemoryDietPlans { get; set; }

        public IList<DietPlanSubListViewModel> CurrentPageDietPlanList
        {
            get
            {
                return DietPlanListViewModel.DietPlanList;
            }
            set
            {
                DietPlanListViewModel.DietPlanList = value;
            }
        }

        public IList<DietPlanSubListViewModel> PreviousPageDietPlanList { get; set; }

        public IList<DietPlanSubListViewModel> NextPageDietPlanList { get; set; }

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
            //switch (PlanType)
            //{
            //    case PlanType.CreatedDietPlan:
            //        InMemoryDietPlans = _foodService.GetAllRecommendedDietPlans().ToList();
            //        break;
            //    case PlanType.RecommendedDietPlan:
            //        InMemoryDietPlans = _foodService.GetAllSavedDietPlansOfUser(_sessionManager.GetCurrentSession().AssociatedUser.Id).ToList();
            //        break;
            //    default:
            //        break;
            //}
            
            //CurrentPage = 1;
            //PageCount = InMemoryDietPlans.Count;
            //CurrentPageDietPlanList = GetDietPlanList(CurrentPage);
            //NextPageDietPlanList = GetDietPlanList(CurrentPage + 1);
        }

        #endregion

        private void ShowPreviousPage()
        {
            if (CurrentPage == 1) return;

            CurrentPage--;
            NextPageDietPlanList = CurrentPageDietPlanList;
            CurrentPageDietPlanList = PreviousPageDietPlanList;
            PreviousPageDietPlanList = GetDietPlanList(CurrentPage - 1);
        }

        private void ShowNextPage()
        {
            if (CurrentPage == PageCount) return;

            CurrentPage++;
            PreviousPageDietPlanList = CurrentPageDietPlanList;
            CurrentPageDietPlanList = NextPageDietPlanList;
            NextPageDietPlanList = GetDietPlanList(CurrentPage + 1);
        }

        private void OnApply()
        {
            if (Apply != null)
                Apply(this, new EventArgs());
        }

        private IList<DietPlanSubListViewModel> GetDietPlanList(int index)
        {
            IList<DietPlanSubListViewModel> dietPlanList = new List<DietPlanSubListViewModel>();

            for (int i = 0; i < 6; i++)
            {
                foreach (var dietPlanItem in InMemoryDietPlans[index].SubDietPlans[i].DietPlanItems)
                {
                    dietPlanList[i].AddFoodItem(new FoodItemViewModel(dietPlanItem.Food)
                        {
                            Amount = dietPlanItem.Amount
                        });
                }
            }

            return dietPlanList;
        }
    }
}
