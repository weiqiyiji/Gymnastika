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
            CategoryItems = new List<ICategoryItemViewModel>();
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                Categories = _foodService.CategoryProvider.GetAll();

                foreach (var category in Categories)
                {
                    ICategoryItemViewModel categoryItem = _container.Resolve<ICategoryItemViewModel>();
                    categoryItem.Category = category;
                    categoryItem.SubCategoryItems = category.SubCategories.ToList();
                    CategoryItems.Add(categoryItem);
                }
            }
            View = view;
            View.Context = this;
            View.CategoryItemSelectionChanged += new SelectionChangedEventHandler(CategoryItemSelectionChanged);
        }

        #region ICategoryListViewModel Members

        public ICategoryListView View { get; set; }

        public IList<ICategoryItemViewModel> CategoryItems { get; set; }

        public Category SelectedCategoryItem { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        #endregion

        private void CategoryItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCategoryItem = ((ICategoryItemViewModel)e.AddedItems[0]).Category;

            _eventAggregator.GetEvent<SelectCategoryEvent>().Publish(SelectedCategoryItem.SubCategories[0]);
        }
    }
}
