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
using Gymnastika.Controls;
using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace Gymnastika.Modules.Sports.ViewModels
{
    //[Export(typeof(ISportsListViewModel))]
    public class SportsListViewModel : NotificationObject, ISportsListViewModel
    {

        public Predicate<object> _sportsFilter;
        public Predicate<object> SportsFilter
        {
            get
            {
                return _sportsFilter;
            }
            set
            {
                if (_sportsFilter != value)
                {
                    _sportsFilter = value;
                    if (SportsView != null) 
                        SportsView.Filter = _sportsFilter;
                }
            }
        }

        public ICollectionView SportsView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(_selectedCategory.Sports);
            }

        }

        string _sportsNameFilter;
        public string SportsNameFilter
        {
            get
            {
              return  _sportsNameFilter; 
            }
            set
            {
                if (_sportsNameFilter != value)
                {
                    _sportsNameFilter = value;
                    ICollectionView view = SportsView;
                    if (view == null)  return;
                    if (_sportsNameFilter == null || _sportsNameFilter == "")
                        view.Filter = null;
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                        (Action)(() =>
                        {
                            view.Filter = (Predicate<object>)(n => (n as Sport).Name.Contains(_sportsNameFilter));
                        }));
                    }
                }
            }
        }

        ObservableCollection<Category> _categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories
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

        Category _selectedCategory = new Category();
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

    }
}