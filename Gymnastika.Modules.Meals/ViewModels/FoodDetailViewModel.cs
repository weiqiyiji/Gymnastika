using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Modules.Meals.Services;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodDetailViewModel : IFoodDetailViewModel
    {
        private readonly IFoodService _foodService;

        public FoodDetailViewModel(IFoodDetailView view, IFoodService foodService)
        {
            _foodService = foodService;
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

        public string CategoryName
        {
            get { return _foodService.GetCategory(Food.Id).Name; }
        }

        public string SubCategoryName
        {
            get { return _foodService.GetSubCategory(Food.Id).Name; }
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

        #endregion
    }
}
