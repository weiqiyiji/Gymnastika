using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodDetailViewModel : IFoodDetailViewModel
    {
        private ICommand _showRelatedFoodsCommand;

        public FoodDetailViewModel(IFoodDetailView view)
        {
            View = view;
            View.Context = this;
        }

        #region IFoodDetailViewModel Members

        public IFoodDetailView View { get; set; }

        public Food Food { get; set; }

        public string Name
        {
            get { return Food.Name; }
        }

        public string ImageUri
        {
            get { return Food.LargeImageUri; }
        }

        public string Calorie
        {
            get { return Food.Calorie.ToString(); }
        }

        public string Category
        {
            get { return Food.Category; }
        }

        public string SubCategory
        {
            get { return Food.SubCategory; }
        }

        public IEnumerable<NutritiveElement> NutritionalContent
        {
            get { return Food.NutritionalContent; }
        }

        public string Introduction
        {
            get { return Food.Introduction; }
        }

        public string NutritionalValue
        {
            get { return Food.NutritionalValue; }
        }

        public string Function
        {
            get { return Food.Function; }
        }

        public string SuitableCrowd
        {
            get { return Food.SuitableCrowd; }
        }

        public IEnumerable<Food> RelatedFoods
        {
            get { return Food.RelatedFoods; }
        }

        #endregion
    }
}
