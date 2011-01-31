using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Data;
using Gymnastika.Modules.Sports.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    [Export(typeof(ISportsListViewModel))]
    public class SportsListViewModel : NotificationObject, ISportsListViewModel
    {

        #region ISportsListViewModel Members
        ObservableCollection<Category> _categories = new ObservableCollection<Category>();
        public System.Collections.ObjectModel.ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    RaisePropertyChanged("Categories");
                }
            }
        }

        #endregion

        #region ISportsListViewModel Members


        Category _selectedCategory;
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                if (value != _selectedCategory)
                {
                    _selectedCategory = value;
                    RaisePropertyChanged("SelectedCategory");
                }
            }
        }

        #endregion
    }
}
