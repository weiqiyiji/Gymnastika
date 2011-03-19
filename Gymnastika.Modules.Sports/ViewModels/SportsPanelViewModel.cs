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
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportEventArgs : EventArgs
    {
        public Sport Sport{get;set;}
    }

    public interface ISportsPanelViewModel
    {
        DelegateCommand NextPageCommand { get; }

        DelegateCommand PreviousPageCommand { get; }

        DelegateCommand SearchCommand { get; }

        ISportCardViewModel SelectedSport { get; set; }

        ObservableCollection<Sport> CurrentSports { get; }

        Func<Sport, bool> Filter { get; set; }

        SportsCategory Category { get; set; }

        IList<Sport> SportsInMemory { get; }

        int CurrentPage { get; }

        int Count { get; }

        int MaxPage { get; }

        event EventHandler<SportEventArgs> RequestAddToFavorateEvent;
        event EventHandler<SportEventArgs> RequestAddToPlanEvent;
        event EventHandler<SportEventArgs> RequestShowDetailEvent;
    }

    public class SportsPanelViewModel : NotificationObject, ISportsPanelViewModel 
    {
        const int MaxItemsPerPage = 5;

        ISportCardViewModelFactory _factory;
        ISportProvider _sportProvider;

        public SportsPanelViewModel(ISportProvider sportprovider, ISportCardViewModelFactory factory)
        {
            _sportProvider = sportprovider;
            _factory = factory;
        }
        DelegateCommand _nextPageCommand;
        public DelegateCommand NextPageCommand
        {
            get 
            {
                if (_nextPageCommand == null)
                    _nextPageCommand = new DelegateCommand(GotoNextPage, CanGotoNextPage);
                return _nextPageCommand;
            }
        }
        DelegateCommand _previousPageCommand;
        public DelegateCommand PreviousPageCommand
        {
            get 
            {
                if (_previousPageCommand == null)
                    _previousPageCommand = new DelegateCommand(GotoPreviousPage, CanGotoPreviousPage);
                return _previousPageCommand;
            }
        }

        DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new DelegateCommand(DoSearch);
                return _searchCommand;
            }
        }

        void DoSearch()
        {
            
            Filter = (s) => s.Name.Contains(SearchName);
            Refresh();
        }

        void UpdateCommandState()
        {
            NextPageCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
        }

        void GotoNextPage()
        {
            GotoPage(CurrentPage + 1);
        }
        bool CanGotoNextPage()
        {
            return CanGotoPage(CurrentPage +1);
        }
        void GotoPreviousPage()
        {
            GotoPage(CurrentPage - 1);
        }
        bool CanGotoPreviousPage()
        {
            return CanGotoPage(CurrentPage - 1);
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

        Func<Sport, bool> _filter;
        public Func<Sport, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    RaisePropertyChanged(() => Filter);
                    
                }
            }
        }
        SportsCategory category;
        public SportsCategory Category
        {
            get { return category; }
            set
            {
                if (value != null && category != value)
                {
                    category = value;
                    InitStates();
                    RaisePropertyChanged();
                }
            }
        }

        IList<Sport> LoadSportsFromRepository(SportsCategory category)
        {
            if (category == null)
                return new List<Sport>();
            using (_sportProvider.GetContextScope())
            {
                var list = _sportProvider.Fetch(t => t.SportsCategories.Contains(category)).ToList();
                return list.ToList();
            }
        }

        private void Refresh()
        {
            SportsInMemory = LoadSportsFromRepository(Category);
            Count = GetCount(Filter);
            MaxPage = GetPageNumber(Count);
            GotoPage(1);
        }

        private void InitStates()
        {
            Filter = null;
            Refresh();
        }

        private int GetPageNumber(int Count)
        {
            return Count / MaxItemsPerPage + ((Count != 0 && Count % MaxItemsPerPage != 0) ? 1 : 0);
        }

        bool CanGotoPage(int page)
        {
            return page <= MaxPage && page >= 1;
        }

        int GetIndexByPage(int page)
        {
            return (page - 1) * MaxItemsPerPage;
        }

        IList<Sport> _sportsInMemory;
        public IList<Sport> SportsInMemory 
        {
            get { return _sportsInMemory; }
            set { _sportsInMemory = value; }
        }

        Func<Sport,bool> InvalidatePredicate(Func<Sport,bool> predicate)
        {
            if (predicate == null)
                predicate = t => true;
            return predicate;
        }
        int GetCount(Func<Sport, bool> predicate)
        {
            return SportsInMemory.Count(InvalidatePredicate(predicate));
        }

        ObservableCollection<Sport> Fetch(int start, int number, Func<Sport,bool> predicate)
        {
            return SportsInMemory.Where(InvalidatePredicate(predicate)).Skip(start).Take(number).ToObservableCollection();
        }

        private bool GotoPage(int page)
        {
            if (CanGotoPage(page))
            {
                CurrentSports = Fetch(GetIndexByPage(page), MaxItemsPerPage, Filter);
                CurrentPage = page;
                UpdateCommandState();
                return true;
            }
            else
            {
                return false;
            }
        }

        int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                if (value != _currentPage)
                {
                    _currentPage = value;
                    RaisePropertyChanged(() => CurrentPage);
                }
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
                }
            }
        }

        public int _maxPage;
        public int MaxPage
        {
            get { return _maxPage; }
            set
            {
                if (_maxPage != value)
                {
                    _maxPage = value;
                    RaisePropertyChanged(() => MaxPage);
                }
            }
        }

        public ICollectionView View
        {
            get { return CollectionViewSource.GetDefaultView(ViewModels); }
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
                    _viewModels.ReplaceBy(value);
                    //_viewModels = value;
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
            model.AddToFavouriteEvent -= OnAddToFavourate;
            model.AddToPlanEvent -= OnAddToPlan;
            model.ShowDetailEvent -= OnShowDetail;
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
            ISportCardViewModel model = _factory.Create(sport);
            model.AddToFavouriteEvent += OnAddToFavourate;
            model.AddToPlanEvent += OnAddToPlan;
            model.ShowDetailEvent += OnShowDetail;
            return model;
        }

        void OnAddToFavourate(object sender, EventArgs args)
        {
            if (RequestAddToFavorateEvent != null)
                RequestAddToFavorateEvent(this, new SportEventArgs() { Sport = (sender as ISportCardViewModel).Sport });
        }
        void OnAddToPlan(object sender, EventArgs args)
        {
            if (RequestAddToPlanEvent != null)
                RequestAddToPlanEvent(this, new SportEventArgs() { Sport = (sender as ISportCardViewModel).Sport });
        }
        void OnShowDetail(object sender, EventArgs args)
        {
            if (RequestShowDetailEvent != null)
                RequestShowDetailEvent(this, new SportEventArgs() { Sport = (sender as ISportCardViewModel).Sport });
        }

        string _searchName = "";
        public string SearchName
        {
            get { return _searchName; }
            set
            {
                if (value != _searchName)
                {
                    _searchName = value;
                    RaisePropertyChanged(() => SearchName);
                }
            }
        }


        ISportCardViewModel _selectedSport;
        public ISportCardViewModel SelectedSport
        {
            get { return _selectedSport; }
            set
            {
                if (_selectedSport != value)
                {
                    _selectedSport = value;
                    RaisePropertyChanged(() => SelectedSport);
                }
            }
        }

        public event EventHandler<SportEventArgs> RequestAddToFavorateEvent = delegate { };

        public event EventHandler<SportEventArgs> RequestAddToPlanEvent = delegate { };

        public event EventHandler<SportEventArgs> RequestShowDetailEvent = delegate { };
    }
}
