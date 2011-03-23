using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using System.ComponentModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IFoodListViewModel
    {
        IFoodListView View { get; set; }
        ICollectionView Category { get; }
        IEnumerable<Food> CurrentFoods { get; set; }
        SubCategory CurrentSubCategory { get; set; }
        FavoriteFood FavoriteFood { get; set; }
        string CategoryName { get; set; }
        ObservableCollection<FoodItemViewModel> CurrentPageFoodList { get; set; }
        ICollection<FoodItemViewModel> MyFavoriteFoodList { get; set; }
        IEnumerable<Food> SearchFoods { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        ICommand ShowPreviousPageCommand { get; }
        ICommand ShowNextPageCommand { get; }
        ICommand ShowMyFavoriteCommand { get; }
        void ShowSearchResult();
        void ShowMyFavoriteResult();
        void SelectCategory(SubCategory subCategory);
    }
}
