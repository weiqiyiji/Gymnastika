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
using Gymnastika.Modules.Sports.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace Gymnastika.Modules.Sports.Views
{
    [Export(typeof(ISportsListViewModel))]
    public class SportsListViewModel : NotificationObject, ISportsListViewModel, IDragSource
    {
        ISportsProvider SportsProvider { set; get; }
        [ImportingConstructor]
        public SportsListViewModel(ISportsProvider provider)
        {
            SportsProvider = provider;
            this.Categories = new ObservableCollection<ISportsCategory>(SportsProvider.SportsCategories);
            if (Categories.Count > 0)
                this.SelectedCategory = Categories[0];
        }

        int _sportsNumPerPage = 5;
        int SportsNumPerPage
        {
            get
            {
                return _sportsNumPerPage;
            }
        }

        int TotalPage
        {
            get
            {
                int total = _selectedCategory.Sports.Count();
                int totalpage = total % SportsNumPerPage == 0 ? total / _sportsNumPerPage : (total / _sportsNumPerPage + 1);
                return totalpage;
            }

        }

        void UpdateSports()
        {
            if (CurrentPage > 0 && CurrentPage < TotalPage)
            {
                CurrentSports = new ObservableCollection<Sport>
                    (SelectedCategory.Sports
                    .Take(CurrentPage*SportsNumPerPage)
                    .Skip((CurrentPage-1)*SportsNumPerPage));
            }
        }

        int _currentPage = 1;
        public int CurrentPage
        {
            set
            {
                if (_currentPage != value && _currentPage > 0 && _currentPage <= TotalPage)
                {
                    _currentPage = value;
                    UpdateSports();
                    RaisePropertyChanged("CurrentPage");
                }
            }
            get
            {
                return _currentPage;
            }
        }

        ObservableCollection<Sport> _currentSports = new ObservableCollection<Sport>();
        public ObservableCollection<Sport> CurrentSports
        {
            get
            {
                return _currentSports;
            }
            set
            {
                if (_currentSports != value)
                {
                    _currentSports = value;
                    RaisePropertyChanged("CurrentSports");
                }
            }
        }

        public Predicate<object> _sportsFilter = delegate { return true; };
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

        void SetSportsNameFilter(string strFilter)
        {
            ICollectionView view = SportsView;
            if (view != null)
            {
                Predicate<object> filter = null;
                if (!String.IsNullOrWhiteSpace(_sportsNameFilter))
                {
                    filter = new Predicate<object>(n => (n as Sport).Name.Contains(_sportsNameFilter));
                }
                Application.Current.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        view.Filter = filter;
                    }));
            }
        }

        string _sportsNameFilter;
        public string SportsNameFilter
        {
            get
            {
                return _sportsNameFilter;
            }
            set
            {
                if (_sportsNameFilter != value)
                {
                    _sportsNameFilter = value;
                    SetSportsNameFilter(_sportsNameFilter);
                    RaisePropertyChanged("SportsNameFilter");
                }
            }
        }

        ObservableCollection<ISportsCategory> _categories = new ObservableCollection<ISportsCategory>();
        public ObservableCollection<ISportsCategory> Categories
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

        ISportsCategory _selectedCategory = null;
        public ISportsCategory SelectedCategory
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

        Sport _selectedSport = new Sport();
        public Sport SelectedSport
        {
            get
            {
                return _selectedSport;
            }
            set
            {
                if (_selectedSport != value)
                {
                    _selectedSport = value;
                    RaisePropertyChanged("SelectedSport");
                }
            }
        }

        #region IDragSource Members

        public void StartDrag(DragInfo dragInfo)
        {
            dragInfo.Data = SelectedSport;
            dragInfo.Effects = DragDropEffects.All;
        }

        #endregion

        ICommand _closeCommand = new DelegateCommand
            (new Action(() => { }),
            new Func<bool>(()=>false))
            {  IsActive = false};
        public ICommand CloseCommand
        {
            set
            {
                if(_closeCommand!=value)
                {
                    _closeCommand = value;
                    RaisePropertyChanged("CloseCommand");
                }
            }
            get
            {
                return _closeCommand;
            }
        }
   
        ICommand _showMoreCommand = new DelegateCommand
            (new Action(() => { }),
            new Func<bool>(()=>false))
            {  IsActive = false};
        public ICommand ShowMoreCommand
        {
            set
            {
                if (_showMoreCommand != value)
                {
                    _showMoreCommand = value;
                    RaisePropertyChanged("ShowMoreCommand");
                }
            }
            get
            {
                return _showMoreCommand;
            }
        }
    }
}