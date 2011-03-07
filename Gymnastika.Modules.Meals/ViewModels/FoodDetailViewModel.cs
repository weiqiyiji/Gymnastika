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

        public string CategoryName
        {
            get { return Food.SubCategory.Name; }
        }

        public string SubCategoryName
        {
            get { return Food.SubCategory.Category.Name; }
        }

        public IEnumerable<NutritionalElement> NutritionalElements
        {
            get { return Food.NutritionalElements; }
        }

        public IEnumerable<Introduction> Introductions
        {
            get { return Food.Introductions; }
        }

        #endregion
    }
}
