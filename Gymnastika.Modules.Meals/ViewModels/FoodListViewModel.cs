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
using Gymnastika.Services.Session;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodListViewModel : NotificationObject, IFoodListViewModel 
    {
        private const int PageSize = 10;
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private int _currentPage;
        private int _pageCount;
        private IEnumerable<Category> _category;
        private ObservableCollection<FoodItemViewModel> _currentPageFoodList;
        private ICommand _showMyFavoriteCommand;
        private ICommand _showPreviousPageCommand;
        private ICommand _showNextPageCommand;

        public FoodListViewModel(IFoodListView view,
            IFoodService foodService,
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                _category = _foodService.CategoryProvider.GetAll();
                foreach (var category in _category)
                {
                    category.SubCategories = _foodService.SubCategoryProvider.GetSubCategories(category).ToList();
                }
                FavoriteFood = _foodService.FavoriteFoodProvider.Get(_sessionManager.GetCurrentSession().AssociatedUser.Id);
                if (FavoriteFood != null)
                    FavoriteFood.Foods = _foodService.FoodProvider.GetFoods(FavoriteFood).ToList();
            }
            Category = CollectionViewSource.GetDefaultView(_category);
            MyFavoriteFoodList = new List<FoodItemViewModel>();
            if (FavoriteFood != null)
            {
                foreach (var food in FavoriteFood.Foods)
                {
                    MyFavoriteFoodList.Add(new FoodItemViewModel(food)
                        {
                            ChangeMyFavoriteButtonContent = "从我的食物库移除"
                        });
                }
            }
            View = view;
            View.Context = this;
            View.SubCategorySelectionChanged += new SelectionChangedEventHandler(SubCategorySelectionChanged);
        }

        #region IFoodListViewModel Members

        public IFoodListView View { get; set; }

        public ICollectionView Category { get; set; }

        public SubCategory CurrentSubCategory { get; set; }

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

        public FavoriteFood FavoriteFood { get; set; }

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

        public void ShowSearchResult()
        {
            if (CurrentFoods.Count() == 0)
            {
                CurrentPage = 0;
                PageCount = 0;
                System.Windows.MessageBox.Show("对不起，没有找到您要的食物");
                return;
            }

            CurrentPage = 1;
            PageCount = (CurrentFoods.Count() % PageSize == 0) ? (CurrentFoods.Count() / PageSize) : (CurrentFoods.Count() / PageSize + 1);

            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }

            //NextPageFoodList = new ObservableCollection<FoodItemViewModel>();
            //foreach (var food in CurrentFoods.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage))
            //{
            //    NextPageFoodList.Add(new FoodItemViewModel(food));
            //}

            AttachAddFoodToMyFavoriteEventHandler();
        }

        #endregion

        private void SubCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSubCategory = (SubCategory)e.AddedItems[0];
            //using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            //{
            //    CurrentFoods = _foodService.FoodProvider.GetFoods(subCategory);
            //}

            CurrentPage = 1;

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                int subTotalCount = _foodService.FoodProvider.Count(CurrentSubCategory);
                PageCount = (subTotalCount % PageSize == 0) ? (subTotalCount / PageSize) : (subTotalCount / PageSize + 1);
                CurrentFoods = _foodService.FoodProvider.GetFoods(CurrentSubCategory, PageSize * (CurrentPage - 1), PageSize);
            }

            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }

            AttachAddFoodToMyFavoriteEventHandler();
        }

        private void AddFoodToMyFavorite(object sender, EventArgs e)
        {
            FoodItemViewModel foodItem = sender as FoodItemViewModel;
            foodItem.ChangeMyFavoriteButtonContent = "从我的食物库移除";
            
            if (MyFavoriteFoodList.FirstOrDefault(f => f.Name == foodItem.Name) == null)
                MyFavoriteFoodList.Add(foodItem);

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                if (FavoriteFood == null)
                {
                    FavoriteFood = new FavoriteFood
                    {
                        User = _sessionManager.GetCurrentSession().AssociatedUser,
                    };
                    _foodService.FavoriteFoodProvider.Create(FavoriteFood);
                    FavoriteFood.Foods = new List<Food>();
                }
                FavoriteFood.Foods.Add(foodItem.Food);
                _foodService.FavoriteFoodProvider.Update(FavoriteFood);
            }
        }

        private void RemoveFoodFromMyFavorite(object sender, EventArgs e)
        {
            FoodItemViewModel foodItem = sender as FoodItemViewModel;

            CurrentPageFoodList.Remove(foodItem);

            MyFavoriteFoodList.Remove(foodItem);

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                FavoriteFood.Foods.Remove(foodItem.Food);
                _foodService.FavoriteFoodProvider.Update(FavoriteFood);
            }
        }

        private void ShowMyFavorite()
        {
            if (MyFavoriteFoodList.Count == 0)
            {
                CurrentPage = 0;
                PageCount = 0;
                System.Windows.MessageBox.Show("您还没有添加食物到自己的食物库");
                return;
            }

            CurrentPage = 1;
            PageCount = (MyFavoriteFoodList.Count % PageSize == 0) ? (MyFavoriteFoodList.Count / PageSize) : (MyFavoriteFoodList.Count / PageSize + 1);
            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1)));
            NextPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage));
            
            AttachRemoveFoodFromMyFavoriteEventHandler();
        }

        private void ShowPreviousPage()
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;
            //NextPageFoodList = CurrentPageFoodList;
            //CurrentPageFoodList = PreviousPageFoodList;

            //PreviousPageFoodList = new ObservableCollection<FoodItemViewModel>();
            //foreach (var food in CurrentFoods.Take(PageSize * (CurrentPage - 1)).Skip(PageSize * (CurrentPage - 2)))
            //{
            //    PreviousPageFoodList.Add(new FoodItemViewModel(food));
            //}

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                CurrentFoods = _foodService.FoodProvider.GetFoods(CurrentSubCategory, PageSize * (CurrentPage - 1), PageSize);
            }

            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }

            AttachAddFoodToMyFavoriteEventHandler();
        }

        private void ShowNextPage()
        {
            if (CurrentPage >= PageCount) return;

            CurrentPage++;
            //PreviousPageFoodList = CurrentPageFoodList;
            //CurrentPageFoodList = NextPageFoodList;

            //NextPageFoodList = new ObservableCollection<FoodItemViewModel>();
            //foreach (var food in CurrentFoods.Take(PageSize * (CurrentPage + 1)).Skip(PageSize * CurrentPage))
            //{
            //    NextPageFoodList.Add(new FoodItemViewModel(food));
            //}

            //CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            //foreach (var food in CurrentFoods.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1)))
            //{
            //    CurrentPageFoodList.Add(new FoodItemViewModel(food));
            //}

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                CurrentFoods = _foodService.FoodProvider.GetFoods(CurrentSubCategory, PageSize * (CurrentPage - 1), PageSize);
            }

            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }

            AttachAddFoodToMyFavoriteEventHandler();
        }

        private void AttachAddFoodToMyFavoriteEventHandler()
        {
            foreach (var foodItem in CurrentPageFoodList)
            {
                foodItem.ChangeMyFavorite += new EventHandler(AddFoodToMyFavorite);
            }
        }

        private void AttachRemoveFoodFromMyFavoriteEventHandler()
        {
            foreach (var foodItem in CurrentPageFoodList)
            {
                foodItem.ChangeMyFavorite += new EventHandler(RemoveFoodFromMyFavorite);
            }
        }
    }
}
