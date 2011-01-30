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
        ICollectionView SubCategory { get; }
        IEnumerable<FoodItemViewModel> InMemoryFoodList { get; set; }
        ObservableCollection<FoodItemViewModel> PreviousPageFoodList { get; set; }
        ObservableCollection<FoodItemViewModel> NextPageFoodList { get; set; }
        ObservableCollection<FoodItemViewModel> CurrentPageFoodList { get; set; }
        ICollection<FoodItemViewModel> MyFavoriteFoodList { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        ICommand ShowMyFavoriteCommand { get; }
        ICommand ShowPreviousPageCommand { get; }
        ICommand ShowNextPageCommand { get; }
        void Update();
    }
}
