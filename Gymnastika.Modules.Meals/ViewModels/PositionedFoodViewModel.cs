using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.ViewModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class PositionedFoodViewModel : NotificationObject, IDropTarget, IPositionedFoodViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public PositionedFoodViewModel(IPositionedFoodView view,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            PositionedFood = new ObservableCollection<FoodItemViewModel>();
            View = view;
            View.Context = this;
        }

        #region IPositionedFoodViewModel Members

        public IPositionedFoodView View { get; set; }

        public ObservableCollection<FoodItemViewModel> PositionedFood { get; set; }

        public string ImageUri { get; set; }

        public string FoodName { get; set; }

        public string Calorie { get; set; }

        #endregion

        #region IDropTarget Members

        void IDropTarget.DragOver(DropInfo dropInfo)
        {
            if (dropInfo.Data is FoodItemViewModel || dropInfo.Data is IEnumerable<FoodItemViewModel>)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(DropInfo dropInfo)
        {
            FoodItemViewModel foodItem = new FoodItemViewModel(((FoodItemViewModel)dropInfo.Data).Food); ;

            if (PositionedFood.Count != 0)
                PositionedFood.Remove(PositionedFood[0]);

            PositionedFood.Add(foodItem);

            foodItem.LoadNutritionElementData();

            _eventAggregator.GetEvent<PositionedFoodNutritionChangedEvent>().Publish(foodItem.NutritionalElements.ToList());
        }

        #endregion
    }
}
