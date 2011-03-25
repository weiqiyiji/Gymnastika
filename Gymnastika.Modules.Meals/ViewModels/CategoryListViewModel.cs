using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class CategoryListViewModel : ICategoryListViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;

        public CategoryListViewModel(ICategoryListView view,
            IEventAggregator eventAggregator,
            IFoodService foodService,
            IWorkEnvironment workEnvironment,
            IUnityContainer container)
        {
            _container = container;
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _eventAggregator = eventAggregator;
            //CategoryItems = new List<CategoryItemViewModel>();
            //FoodListViewModels = new List<IFoodListViewModel>();
            FoodListViewModel = ServiceLocator.Current.GetInstance<IFoodListViewModel>();
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                Categories = _foodService.CategoryProvider.GetAll();
            }
            View = view;
            View.Context = this;
            View.CategoryItemSelectionChanged += new SelectionChangedEventHandler(CategoryItemSelectionChanged);
        }

        #region ICategoryListViewModel Members

        public ICategoryListView View { get; set; }

        public IList<CategoryItemViewModel> CategoryItems { get; set; }

        //public IList<IFoodListViewModel> FoodListViewModels { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }

        public Category SelectedCategoryItem { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        #endregion

        private void CategoryItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCategoryItem = ((Category)e.AddedItems[0]);

            _eventAggregator.GetEvent<SelectCategoryEvent>().Publish(SelectedCategoryItem);
        }
    }
}
