using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class FoodDetailViewModel : IFoodDetailViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;

        public FoodDetailViewModel(IFoodDetailView view,
            IFoodService foodService,
            IWorkEnvironment workEnvironment)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
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
            get { return Decimal.Round(Food.Calorie).ToString(); }
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

        public void Initialize()
        {
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                Food.NutritionalElements = _foodService.NutritionalElementProvider.GetNutritionalElements(Food).ToList();
                Food.Introductions = _foodService.IntroductionProvider.GetIntroductions(Food).ToList();
                Food.SubCategory = _foodService.SubCategoryProvider.Get(Food);
                Food.SubCategory.Category = _foodService.CategoryProvider.Get(Food.SubCategory);
            }
        }

        #endregion
    }
}
