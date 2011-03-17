using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Gymnastika.Modules.Meals.Models;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class CategoryItemViewModel : ICategoryItemViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public CategoryItemViewModel(ICategoryItemView view, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            View = view;
            View.Context = this;
            View.SubCategoryItemSelectionChanged += new SelectionChangedEventHandler(SubCategoryItemSelectionChanged);
        }

        #region ICategoryItemViewModel Members

        public ICategoryItemView View { get; set; }

        public IEnumerable<SubCategory> SubCategoryItems { get; set; }

        public SubCategory SelectedSubCategoryItem { get; set; }

        public Category Category { get; set; }

        public string ImageUri { get { return Category.ImageUri; } }

        public string Name { get { return Category.Name; } }

        #endregion

        private void SubCategoryItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSubCategoryItem = (SubCategory)e.AddedItems[0];

            _eventAggregator.GetEvent<SelectCategoryEvent>().Publish(SelectedSubCategoryItem);
        }
    }
}
