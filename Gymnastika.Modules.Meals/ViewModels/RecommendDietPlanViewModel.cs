using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class RecommendDietPlanViewModel : NotificationObject, IRecommendDietPlanViewModel
    {
        public const int FoodNumber = 10;
        private int _currentPage;
        private int _pageCount;
        private ICommand _showPreviousPageCommand;
        private ICommand _showNextPageCommand;
        private ICommand _applyCommand;

        public RecommendDietPlanViewModel(IRecommendDietPlanView view, IDietPlanListViewModel dietPlanListViewModel)
        {
            DietPlanListViewModel = dietPlanListViewModel;
            View = view;
            View.Context = this;
            //InMemoryRecommendedDietPlan = new List<RecommendedDietPlan>();
            Initialize();
        }

        #region IRecommendDietPlanViewModel Members

        public IRecommendDietPlanView View { get; set; }

        public IDietPlanListViewModel DietPlanListViewModel { get; set; }

        public IList<RecommendedDietPlan> InMemoryRecommendedDietPlan { get; set; }

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

        #endregion

        private void Initialize()
        {
            CurrentPage = 1;
            PageCount = InMemoryRecommendedDietPlan.Count;
            CurrentPageDietPlanList = GetRecommendedDietPlanList(CurrentPage);
            NextPageDietPlanList = GetRecommendedDietPlanList(CurrentPage + 1);
        }

        private void ShowPreviousPage()
        {
            if (CurrentPage == 1) return;

            CurrentPage--;
            NextPageDietPlanList = CurrentPageDietPlanList;
            CurrentPageDietPlanList = PreviousPageDietPlanList;
            PreviousPageDietPlanList = GetRecommendedDietPlanList(CurrentPage - 1);
        }

        private void ShowNextPage()
        {
            if (CurrentPage == PageCount) return;

            CurrentPage++;
            PreviousPageDietPlanList = CurrentPageDietPlanList;
            CurrentPageDietPlanList = NextPageDietPlanList;
            NextPageDietPlanList = GetRecommendedDietPlanList(CurrentPage + 1);
        }

        private void OnApply()
        {
            if (Apply != null)
                Apply(this, new EventArgs());
        }

        private IList<DietPlanSubListViewModel> GetRecommendedDietPlanList(int index)
        {
            IList<DietPlanSubListViewModel> dietPlanList = new List<DietPlanSubListViewModel>();

            for (int i = 0; i < 6; i++)
            {
                foreach (var food in InMemoryRecommendedDietPlan[index].Foods.Take(FoodNumber * (i + 1)).Skip(FoodNumber * i))
                {
                    dietPlanList[i].AddFoodItem(new FoodItemViewModel(food));
                }
            }

            return dietPlanList;
        }
    }
}
