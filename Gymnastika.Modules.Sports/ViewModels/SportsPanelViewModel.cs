using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.Events;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Extensions;
using GongSolutions.Wpf.DragDrop;
using Gymnastika.Modules.Sports.Services;
using System.Windows.Data;
using System.ComponentModel;
using Gymnastika.Modules.Sports.Services.Factories;
using Gymnastika.Modules.Sports.Services.Providers;
using Microsoft.Practices.Unity;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsPanelViewModel
    {
        ObservableCollection<Sport> CurrentSports { get; }

        Func<Sport,bool> Filter { get; set; }

        SportsCategory Category { get; set; }

        int CurrentPage { get; }
    }

    public class SportsPanelViewModel : NotificationObject, ISportsPanelViewModel
    {
        const int MaxItemsPerPage = 10;

        ISportCardViewModelFactory _factory;
        ISportProvider _sportProvider;

        public SportsPanelViewModel(ISportProvider sportprovider,ISportCardViewModelFactory factory)
        {
            _sportProvider = sportprovider;
            _factory = factory;
        }

        #region properties

        int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value && GoToPage(value))
                {
                    RaisePropertyChanged(() => CurrentPage);
                }
            }
        }

        public int MaxPage
        {
            get
            {
                return Count / MaxItemsPerPage + 1;
            }
        }

        int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged(() => Count);
                    RaisePropertyChanged(() => MaxPage);
                }
            }
        }



        #endregion

        void UpdateCount()
        {
            Count = GetCount(Filter);
        }

        int GetCount(Func<Sport, bool> predicate)
        {
            if (Category != null)
            {
                using (_sportProvider.GetContextScope())
                {
                    return _sportProvider.Fetch(Category, Filter).Count();
                }
            }
            else
                return 1;
        }



        int GetStartIndex(int page)
        {
            page = page - 1;
            return (page < 0 ? 0 : page) * MaxItemsPerPage;
        }

        private bool GoToPage(int page)
        {
            if (page >= 1 && page != CurrentPage && page <= MaxPage)
            {
                using(_sportProvider.GetContextScope())
                {
                    CurrentSports = _sportProvider.Fetch
                        (Category, GetStartIndex(page), MaxItemsPerPage, Filter)
                        .ToObservableCollection();
                }
                CurrentPage = page;
                return true;
            }
            return false;
        }

        public ICollectionView View
        {
            get { return CollectionViewSource.GetDefaultView(ViewModels); }
        }

        Func<Sport,bool> _filter;
        public Func<Sport, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    RaisePropertyChanged(() => Filter);
                    View.Filter = s => _filter((s as ISportCardViewModel).Sport);
                }
            }
        }

        string _searchName;
        public string SearchName
        {
            get { return _searchName; }
            set
            {
                if (value != _searchName)
                {
                    _searchName = value;
                    RaisePropertyChanged(() => SearchName);
                    Filter = (s) => s.Name.Contains(_searchName);
                    GoToPage(1);
                    View.Refresh();
                }
            }
        }

        SportsCategory _category = new SportsCategory() { Sports = new List<Sport>() };
        public SportsCategory Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != null && _category != value)
                {
                    _category = value;
                    UpdateState();
                    RaisePropertyChanged(() => Category);

                }
            }
        }


        void ResetFilter() { Filter = (s) => true; }

        private void UpdateState()
        {
            ResetFilter();
            UpdateCount();
            GoToPage(1);
        }

        ObservableCollection<Sport> _currentSports;
        public ObservableCollection<Sport> CurrentSports
        {
            get { return _currentSports; }
            set
            {
                if (_currentSports != value)
                {
                    
                    _currentSports = value;
                    ViewModels = GetViewModels(_currentSports);
                    RaisePropertyChanged(() => CurrentSports);
                }
            }
        }

        private ObservableCollection<ISportCardViewModel> GetViewModels(IList<Sport> sports)
        {
            ObservableCollection<ISportCardViewModel> viewModels = new ObservableCollection<ISportCardViewModel>();
            foreach (Sport sport in sports)
            {
                ISportCardViewModel viewmodel = CreateViewModel(sport);
                viewModels.Add(viewmodel);
            }
            return viewModels;
        }

        ISportCardViewModel CreateViewModel(Sport sport)
        {
            return _factory.Create(sport);
        }

        ObservableCollection<ISportCardViewModel> _viewModels = new ObservableCollection<ISportCardViewModel>();
        public ObservableCollection<ISportCardViewModel> ViewModels
        {
            get { return _viewModels; }
            set
            {
                if (value != null && value != _viewModels)
                {
                    ReleaseViewModels(_viewModels);
                    _viewModels = value;
                    RaisePropertyChanged(() => ViewModels);
                }
            }
        }

        void ReleaseViewModels(IEnumerable<ISportCardViewModel> models)
        {
            if (models == null)
                return;
            foreach (ISportCardViewModel model in models)
            {
                ReleaseViewModel(model);
            }
        }

        void ReleaseViewModel(ISportCardViewModel model)
        {
            //Remove Handlers
        }
    }
}
