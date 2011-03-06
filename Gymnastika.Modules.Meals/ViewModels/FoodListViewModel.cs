using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Models;
using System.ComponentModel;
using System.Windows.Data;
using Gymnastika.Modules.Meals.Services;
using System.Windows.Controls;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodListViewModel : NotificationObject, IFoodListViewModel 
    {
        private const int PageSize = 10;
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private int _currentPage;
        private int _pageCount;
        private IEnumerable<Category> _category;
        private ObservableCollection<FoodItemViewModel> _currentPageFoodList;
        private ICommand _showMyFavoriteCommand;
        private ICommand _showPreviousPageCommand;
        private ICommand _showNextPageCommand;

        public FoodListViewModel(IFoodListView view,
            IFoodService foodService,
            IWorkEnvironment workEnvironment)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _category = _foodService.CategoryProvider.GetAll();
            Category = CollectionViewSource.GetDefaultView(_category);
            MyFavoriteFoodList = new List<FoodItemViewModel>();
            View = view;
            View.Context = this;
            View.SubCategorySelectionChanged += new SelectionChangedEventHandler(SubCategorySelectionChanged);
        }

        #region IFoodListViewModel Members

        public IFoodListView View { get; set; }

        public ICollectionView Category { get; set; }

        public IEnumerable<Food> CurrentFoods { get; set; }

        public ObservableCollection<FoodItemViewModel> PreviousPageFoodList { get; set; }

        public ObservableCollection<FoodItemViewModel> NextPageFoodList { get; set; }

        public ObservableCollection<FoodItemViewModel> CurrentPageFoodList
        {
            get
            {
                return _currentPageFoodList;
            }
            set
            {
                if (_currentPageFoodList != value)
                {
                    _currentPageFoodList = value;
                    RaisePropertyChanged("CurrentPageFoodList");
                }
            }
        }

        public ICollection<FoodItemViewModel> MyFavoriteFoodList { get; set; }

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

        public ICommand ShowMyFavoriteCommand
        {
            get
            {
                if (_showMyFavoriteCommand == null)
                    _showMyFavoriteCommand = new DelegateCommand(ShowMyFavorite);

                return _showMyFavoriteCommand;
            }
        }

        public void Initialize()
        {
            CurrentPage = 1;
            PageCount = (CurrentFoods.Count() % PageSize == 0) ? (CurrentFoods.Count() / PageSize) : (CurrentFoods.Count() / PageSize + 1);
            
            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }
            
            NextPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage))
            {
                NextPageFoodList.Add(new FoodItemViewModel(food));
            }

            Refresh();
        }

        #endregion

        private void SubCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFoods = ((SubCategory)e.AddedItems[0]).Foods;

            Initialize();
        }

        private void AddFoodToMyFavorite(object sender, EventArgs e)
        {
            FoodItemViewModel foodItem = sender as FoodItemViewModel;
            if (MyFavoriteFoodList.FirstOrDefault(f => f.Name == foodItem.Name) == null)
                MyFavoriteFoodList.Add(foodItem);
        }

        private void ShowMyFavorite()
        {
            CurrentPage = 1;
            PageCount = (MyFavoriteFoodList.Count % PageSize == 0) ? (MyFavoriteFoodList.Count / PageSize) : (MyFavoriteFoodList.Count / PageSize + 1);
            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1)));
            NextPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage));
            Refresh();
        }

        private void ShowPreviousPage()
        {
            if (CurrentPage == 1) return;

            CurrentPage--;
            NextPageFoodList = CurrentPageFoodList;
            CurrentPageFoodList = PreviousPageFoodList;

            PreviousPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods.Take(PageSize * (CurrentPage - 1)).Skip(PageSize * (CurrentPage - 2)))
            {
                PreviousPageFoodList.Add(new FoodItemViewModel(food));
            }

            Refresh();
        }

        private void ShowNextPage()
        {
            if (CurrentPage == PageCount) return;

            CurrentPage++;
            PreviousPageFoodList = CurrentPageFoodList;
            CurrentPageFoodList = NextPageFoodList;

            NextPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage))
            {
                NextPageFoodList.Add(new FoodItemViewModel(food));
            }

            Refresh();
        }

        private void Refresh()
        {
            foreach (var foodItem in CurrentPageFoodList)
            {
                foodItem.AddFoodToMyFavorite += new EventHandler(AddFoodToMyFavorite);
            }
        }
    }
}
