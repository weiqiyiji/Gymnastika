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
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodListViewModel : NotificationObject, IFoodListViewModel 
    {
        private const int PageSize = 10;
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private readonly IEventAggregator _eventAggregator;
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
            ISessionManager sessionManager,
            IEventAggregator eventAggregator)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;
            
            View = view;
            View.Context = this;
            View.FoodItemSelectionChanged += new SelectionChangedEventHandler(FoodItemSelectionChanged);
            _eventAggregator.GetEvent<SelectCategoryEvent>().Subscribe(SelectCategory);
        }

        #region IFoodListViewModel Members

        public IFoodListView View { get; set; }

        public ICollectionView Category { get; set; }

        public Category CurrentCategory { get; set; }

        public IEnumerable<Food> CurrentFoods { get; set; }

        public string CategoryName { get; set; }

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

        public IEnumerable<Food> SearchFoods { get; set; }

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

        public FoodListType ListType { get; set; }

        public void ShowSearchResult()
        {
            if (SearchFoods.Count() == 0)
            {
                CurrentPage = 0;
                PageCount = 0;
                System.Windows.MessageBox.Show("对不起，没有找到您要的食物");
                return;
            }

            ListType = FoodListType.Searched;

            CurrentPage = 1;
            PageCount = (SearchFoods.Count() % PageSize == 0) ? (SearchFoods.Count() / PageSize) : (SearchFoods.Count() / PageSize + 1);
            
            CurrentFoods = SearchFoods.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1));
            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }

            AttachAddFoodToMyFavoriteEventHandler();
        }

        public void ShowMyFavoriteResult()
        {
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                FavoriteFood = _foodService.FavoriteFoodProvider.Get(_sessionManager.GetCurrentSession().AssociatedUser.Id);
                if (FavoriteFood != null)
                    FavoriteFood.Foods = _foodService.FoodProvider.GetFoods(FavoriteFood).ToList();
            }

            MyFavoriteFoodList = new List<FoodItemViewModel>();
            if (FavoriteFood != null)
            {
                foreach (var food in FavoriteFood.Foods)
                {
                    MyFavoriteFoodList.Add(new FoodItemViewModel(food)
                    {
                        ChangeMyFavoriteButtonContent = "- 删除"
                    });
                }
            }
            if (MyFavoriteFoodList.Count == 0)
            {
                System.Windows.MessageBox.Show("您还没有添加食物到自己的食物库");
                return;
            }

            ListType = FoodListType.MyFavorite;

            CurrentPage = 1;
            PageCount = (MyFavoriteFoodList.Count % PageSize == 0) ? (MyFavoriteFoodList.Count / PageSize) : (MyFavoriteFoodList.Count / PageSize + 1);
            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1)));

            AttachRemoveFoodFromMyFavoriteEventHandler();
        }

        public void SelectCategory(Category category)
        {
            CurrentCategory = category;

            ListType = FoodListType.Category;

            CurrentPage = 1;

            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                int subTotalCount = _foodService.FoodProvider.Count(CurrentCategory);
                PageCount = (subTotalCount % PageSize == 0) ? (subTotalCount / PageSize) : (subTotalCount / PageSize + 1);
                CurrentFoods = _foodService.FoodProvider.GetFoods(CurrentCategory, PageSize * (CurrentPage - 1), PageSize);
            }

            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
            foreach (var food in CurrentFoods)
            {
                CurrentPageFoodList.Add(new FoodItemViewModel(food));
            }

            AttachAddFoodToMyFavoriteEventHandler();
        }

        #endregion

        public enum FoodListType
        {
            Category = 0,
            MyFavorite = 1,
            Searched = 2
        }

        private void FoodItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            FoodItemViewModel foodItem = (FoodItemViewModel)e.AddedItems[0];
            IList<NutritionElement> nutritions;
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                nutritions = _foodService.NutritionElementProvider.GetNutritionElements(foodItem.Food).ToList();
            }
            _eventAggregator.GetEvent<SelectedFoodNutritionChangedEvent>().Publish(nutritions);
        }

        private void AddFoodToMyFavorite(object sender, EventArgs e)
        {
            //if (MyFavoriteFoodList == null)
            //    MyFavoriteFoodList = new List<FoodItemViewModel>();

            FoodItemViewModel foodItem = sender as FoodItemViewModel;
            
            //if (MyFavoriteFoodList.FirstOrDefault(f => f.Name == foodItem.Name) == null)
            //    MyFavoriteFoodList.Add(foodItem);

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

            System.Windows.MessageBox.Show("添加成功");
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
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                FavoriteFood = _foodService.FavoriteFoodProvider.Get(_sessionManager.GetCurrentSession().AssociatedUser.Id);
                if (FavoriteFood != null)
                    FavoriteFood.Foods = _foodService.FoodProvider.GetFoods(FavoriteFood).ToList();
            }

            MyFavoriteFoodList = new List<FoodItemViewModel>();

            if (FavoriteFood != null)
            {
                foreach (var food in FavoriteFood.Foods)
                {
                    MyFavoriteFoodList.Add(new FoodItemViewModel(food)
                    {
                        ChangeMyFavoriteButtonContent = "- 删除"
                    });
                }
            }

            if (MyFavoriteFoodList.Count == 0)
            {
                System.Windows.MessageBox.Show("您还没有添加食物到自己的食物库");
                return;
            }

            ListType = FoodListType.MyFavorite;

            CurrentPage = 1;
            PageCount = (MyFavoriteFoodList.Count % PageSize == 0) ? (MyFavoriteFoodList.Count / PageSize) : (MyFavoriteFoodList.Count / PageSize + 1);
            CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1)));

            AttachRemoveFoodFromMyFavoriteEventHandler();
        }

        private void ShowPreviousPage()
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;

            ChangePage();
        }

        private void ShowNextPage()
        {
            if (CurrentPage >= PageCount) return;

            CurrentPage++;

            ChangePage();
        }

        private void ChangePage()
        {
            switch (ListType)
            {
                case FoodListType.Category:
                    using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
                    {
                        CurrentFoods = _foodService.FoodProvider.GetFoods(CurrentCategory, PageSize * (CurrentPage - 1), PageSize);
                    }
                    CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
                    foreach (var food in CurrentFoods)
                    {
                        CurrentPageFoodList.Add(new FoodItemViewModel(food));
                    }
                    break;
                case FoodListType.MyFavorite:
                    CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>(MyFavoriteFoodList.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1)));
                    break;
                case FoodListType.Searched:
                    CurrentFoods = SearchFoods.Take(PageSize * CurrentPage).Skip(PageSize * (CurrentPage - 1));
                    CurrentPageFoodList = new ObservableCollection<FoodItemViewModel>();
                    foreach (var food in CurrentFoods)
                    {
                        CurrentPageFoodList.Add(new FoodItemViewModel(food));
                    }
                    break;
                default:
                    break;
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
