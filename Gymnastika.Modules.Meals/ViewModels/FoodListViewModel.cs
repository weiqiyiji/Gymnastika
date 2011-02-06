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

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodListViewModel : NotificationObject, IFoodListViewModel 
    {
        private const int PageSize = 10;
        private int _currentPage;
        private int _pageCount;
        private IEnumerable<Category> _category;
        private IEnumerable<SubCategory> _subCategory;
        private ICommand _showMyFavoriteCommand;
        private ICommand _showPreviousPageCommand;
        private ICommand _showNextPageCommand;

        public FoodListViewModel(IFoodListView view)
        {
            View = view;
            View.Context = this;
            //InMemoryFoodList = new Collection<FoodItemViewModel>();
            Initialize();
        }

        #region IFoodListViewModel Members

        public IFoodListView View { get; set; }

        public ICollectionView Category
        {
            get
            {
                //if (_category == null)
                //    _category = ...

                return CollectionViewSource.GetDefaultView(_category);
            }
        }

        public ICollectionView SubCategory
        {
            get
            {
                _subCategory = ((Category)Category.CurrentItem).SubCategory;

                return CollectionViewSource.GetDefaultView(_subCategory);
            }
        }

        public IEnumerable<FoodItemViewModel> InMemoryFoodList { get; set; }

        public ObservableCollection<FoodItemViewModel> PreviousPageFoodList { get; set; }

        public ObservableCollection<FoodItemViewModel> NextPageFoodList { get; set; }

        public ObservableCollection<FoodItemViewModel> CurrentPageFoodList { get; set; }

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
            PageCount = (InMemoryFoodList.Count() % PageSize == 0) ? (InMemoryFoodList.Count() / PageSize) : (InMemoryFoodList.Count() / PageSize + 1);
            CurrentPageFoodList = (ObservableCollection<FoodItemViewModel>)InMemoryFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1));
            NextPageFoodList = (ObservableCollection<FoodItemViewModel>)InMemoryFoodList.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage);
            Refresh();
        }

        #endregion

        private void AddFoodToMyFavorite(object sender, EventArgs e)
        {
            FoodItemViewModel foodItem = sender as FoodItemViewModel;
            MyFavoriteFoodList.Add(foodItem);
        }

        private void ShowMyFavorite()
        {
            CurrentPage = 1;
            PageCount = (MyFavoriteFoodList.Count % PageSize == 0) ? (MyFavoriteFoodList.Count / PageSize) : (MyFavoriteFoodList.Count / PageSize + 1);
            CurrentPageFoodList = (ObservableCollection<FoodItemViewModel>)MyFavoriteFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1));
            NextPageFoodList = (ObservableCollection<FoodItemViewModel>)MyFavoriteFoodList.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage);
            Refresh();
        }

        private void ShowPreviousPage()
        {
            if (CurrentPage == 1) return;

            CurrentPage--;
            NextPageFoodList = CurrentPageFoodList;
            CurrentPageFoodList = PreviousPageFoodList;
            PreviousPageFoodList = (ObservableCollection<FoodItemViewModel>)InMemoryFoodList.Take(PageSize * (CurrentPage - 1)).Skip(PageSize * (CurrentPage - 2));
            Refresh();
        }

        private void ShowNextPage()
        {
            if (CurrentPage == PageCount) return;

            CurrentPage++;
            PreviousPageFoodList = CurrentPageFoodList;
            CurrentPageFoodList = NextPageFoodList;
            NextPageFoodList = (ObservableCollection<FoodItemViewModel>)InMemoryFoodList.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage);
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
