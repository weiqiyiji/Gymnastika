using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodItemViewModel : NotificationObject
    {
        private int _amount;
        private int _calories;
        private ICommand _deleteFoodFromPlanCommand;
        private ICommand _addFoodToMyFavoriteCommand;
        private ICommand _showFoodDetailCommand;

        public FoodItemViewModel(Food food)
        {
            Food = food;
            _calories = 0;
        }

        public Food Food { get; set; }

        public string Name
        {
            get { return Food.Name; }
        }

        public string SmallImageUri
        {
            get { return Food.SmallImageUri; }
        }

        public string LargeImageUri
        {
            get { return Food.LargeImageUri; }
        }

        public int Calorie
        {
            get { return Food.Calorie; }
        }

        public int Amount
        {
            get 
            { 
                return _amount; 
            }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    RaisePropertyChanged("Amount");
                    Calories = Calorie * Amount;
                }
            }
        }

        public int Calories
        {
            get
            {
                return _calories;
            }
            set
            {
                if (_calories != value)
                {
                    _calories = value;
                    RaisePropertyChanged("Calories");
                    OnDietPlanSubListPropertyChanged();
                }
            }
        }

        public ICommand DeleteFoodFromPlanCommand
        {
            get
            {
                if (_deleteFoodFromPlanCommand == null)
                    _deleteFoodFromPlanCommand = new DelegateCommand(OnDeleteFoodFromPlan);

                return _deleteFoodFromPlanCommand;
            }
        }

        public ICommand AddFoodToMyFavoriteCommand
        {
            get
            {
                if (_addFoodToMyFavoriteCommand == null)
                    _addFoodToMyFavoriteCommand = new DelegateCommand(OnAddFoodToMyFavorite);

                return _addFoodToMyFavoriteCommand;
            }
        }

        public ICommand ShowFoodDetailCommand
        {
            get
            {
                if (_showFoodDetailCommand != null)
                    _showFoodDetailCommand = new DelegateCommand(ShowFoodDetail);

                return _showFoodDetailCommand;
            }
        }

        public event EventHandler DeleteFoodFromPlan;

        public event EventHandler AddFoodToMyFavorite;

        public event EventHandler DietPlanSubListPropertyChanged;

        private void OnDeleteFoodFromPlan()
        {
            if (DeleteFoodFromPlan != null)
                DeleteFoodFromPlan(this, new EventArgs());
        }

        private void OnAddFoodToMyFavorite()
        {
            if (AddFoodToMyFavorite != null)
                AddFoodToMyFavorite(this, new EventArgs());
        }

        private void OnDietPlanSubListPropertyChanged()
        {
            if (DietPlanSubListPropertyChanged != null)
                DietPlanSubListPropertyChanged(this, new EventArgs());
        }

        private void ShowFoodDetail()
        {
            IFoodDetailViewModel foodDetailViewModel = ServiceLocator.Current.GetInstance<IFoodDetailViewModel>();
            foodDetailViewModel.Food = Food;
            foodDetailViewModel.View.ShowView();
        }
    }
}
