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

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsPanelViewModel
    {
        ICommand NextPageCommand { get; }

        ICommand PreviousPageCommand { get; }

        ObservableCollection<Sport> CurrentSports { get; }

        Func<Sport, bool> Filter { get; set; }

        SportsCategory Category { get; set; }

        int CurrentPage { get; }

        int Count { get; }

        int MaxPage { get; }
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

        ICommand _nextPageCommand;
        public ICommand NextPageCommand
        {
            get 
            {
                if (_nextPageCommand == null)
                    _nextPageCommand = new DelegateCommand(GotoNextPage, CanGotoNextPage);
                return _nextPageCommand;
            }
        }
        ICommand _previousPageCommand;
        public ICommand PreviousPageCommand
        {
            get 
            {
                if (_previousPageCommand == null)
                    _previousPageCommand = new DelegateCommand(GotoPreviousPage, CanGotoPreviousPage);
                return _previousPageCommand;
            }
        }
        void UpdateCommandState()
        {
            RaisePropertyChanged(() => NextPageCommand);
            RaisePropertyChanged(() => PreviousPageCommand);
        }

        void GotoNextPage()
        {
            GotoPage(CurrentPage + 1);
            UpdateCommandState();
        }
        bool CanGotoNextPage()
        {
            return CanGotoPage(CurrentPage +1);
        }
        void GotoPreviousPage()
        {
            GotoPage(CurrentPage - 1);
            UpdateCommandState();
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
            get
            {
                return category;
            }
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



        private void InitStates()
        {
            Filter = null;
            Count = GetCount(Filter);
            MaxPage = GetPageNumber(Count);
            GotoPage(1);
        }

        private int GetPageNumber(int Count)
        {
            return Count / MaxItemsPerPage + 1;
        }

        

        int GetCount(Func<Sport,bool> predicate)
        {
            using (_sportProvider.GetContextScope())
            {
                return _sportProvider.Count(predicate);
            }
        }

        bool CanGotoPage(int page)
        {
            return page <= MaxPage && page >= 1;
        }

        int GetIndexByPage(int page)
        {
            return (page - 1) * MaxItemsPerPage;
        }

        ObservableCollection<Sport> Fetch(int start, int number, Func<Sport,bool> predicate)
        {
            return _sportProvider.Fetch(start, number, predicate).ToObservableCollection();
        }

        private bool GotoPage(int page)
        {
            if (CanGotoPage(page))
            {
                using(_sportProvider.GetContextScope())
                {
                    CurrentSports = Fetch(GetIndexByPage(page), MaxItemsPerPage, Filter);
                    
                }
                CurrentPage = page;
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

        }
        void OnAddToPlan(object sender, EventArgs args)
        {

        }
        void OnShowDetail(object sender, EventArgs args)
        {

        }

        //    #region properties

        //    int _currentPage = 1;
        //    public int CurrentPage
        //    {
        //        get { return _currentPage; }
        //        set
        //        {
        //            if (_currentPage != value && CanGotoPage(value))
        //            {
        //                _currentPage = value;
        //                GoToPage(_currentPage);
        //                RaisePropertyChanged(() => CurrentPage);
        //            }
        //        }
        //    }

        //    public int MaxPage
        //    {
        //        get
        //        {
        //            return Count / MaxItemsPerPage + 1;
        //        }
        //    }

        //    int _count;
        //    public int Count
        //    {
        //        get { return _count; }
        //        set
        //        {
        //            if (_count != value)
        //            {
        //                _count = value;
        //                RaisePropertyChanged(() => Count);
        //                RaisePropertyChanged(() => MaxPage);
        //            }
        //        }
        //    }



        //    #endregion

        //    void UpdateCount()
        //    {
        //        Count = GetCount(Filter);
        //    }

        //    int GetCount(Func<Sport, bool> predicate)
        //    {
        //        if (Category != null)
        //        {
        //            using (_sportProvider.GetContextScope())
        //            {
        //                return _sportProvider.Fetch(Category, Filter).Count();
        //            }
        //        }
        //        else
        //            return 1;
        //    }



        //    int GetStartIndex(int page)
        //    {
        //        page = page - 1;
        //        return (page < 0 ? 0 : page) * MaxItemsPerPage;
        //    }

        //    bool CanGotoPage(int page)
        //    {
        //        return page >= 1 &&  page <= MaxPage;
        //    }

        //    private bool GoToPage(int page)
        //    {
        //        if (CanGotoPage(page))
        //        {
        //            using(_sportProvider.GetContextScope())
        //            {
        //                CurrentSports = _sportProvider.Fetch
        //                    (Category, GetStartIndex(page), MaxItemsPerPage, Filter)
        //                    .ToObservableCollection();
        //            }
        //            CurrentPage = page;
        //            return true;
        //        }
        //        return false;
        //    }

        //    public ICollectionView View
        //    {
        //        get { return CollectionViewSource.GetDefaultView(ViewModels); }
        //    }

        //    Func<Sport,bool> _filter;
        //    public Func<Sport, bool> Filter
        //    {
        //        get { return _filter; }
        //        set
        //        {
        //            if (_filter != value)
        //            {
        //                _filter = value;
        //                UpdateCount();
        //                RaisePropertyChanged(() => Filter);
        //                View.Filter = s => _filter((s as ISportCardViewModel).Sport);
        //                View.Refresh();
        //            }
        //        }
        //    }

        //    string _searchName;
        //    public string SearchName
        //    {
        //        get { return _searchName; }
        //        set
        //        {
        //            if (value != _searchName)
        //            {
        //                _searchName = value;
        //                RaisePropertyChanged(() => SearchName);
        //                Filter = (s) => s.Name.Contains(_searchName);
        //                GoToPage(1);
        //            }
        //        }
        //    }

        //    SportsCategory _category = new SportsCategory() { Sports = new List<Sport>() };
        //    public SportsCategory Category
        //    {
        //        get
        //        {
        //            return _category;
        //        }
        //        set
        //        {
        //            if (_category != null && _category != value)
        //            {
        //                _category = value;
        //                ResetAll();
        //                RaisePropertyChanged(() => Category);

        //            }
        //        }
        //    }

        //    void ResetAll()
        //    {
        //        UpdateState();
        //    }

        //    void ResetFilter() { Filter = (s) => true; }

        //    private void UpdateState()
        //    {
        //        _searchName = "";

        //    }

        //    ObservableCollection<Sport> _currentSports;
        //    public ObservableCollection<Sport> CurrentSports
        //    {
        //        get { return _currentSports; }
        //        set
        //        {
        //            if (_currentSports != value)
        //            {

        //                _currentSports = value;
        //                ViewModels = GetViewModels(_currentSports);
        //                RaisePropertyChanged(() => CurrentSports);
        //            }
        //        }
        //    }

        //    private ObservableCollection<ISportCardViewModel> GetViewModels(IList<Sport> sports)
        //    {
        //        ObservableCollection<ISportCardViewModel> viewModels = new ObservableCollection<ISportCardViewModel>();
        //        foreach (Sport sport in sports)
        //        {
        //            ISportCardViewModel viewmodel = CreateViewModel(sport);
        //            viewModels.Add(viewmodel);
        //        }
        //        return viewModels;
        //    }

        //    ISportCardViewModel CreateViewModel(Sport sport)
        //    {
        //        return _factory.Create(sport);
        //    }

        //    ObservableCollection<ISportCardViewModel> _viewModels = new ObservableCollection<ISportCardViewModel>();
        //    public ObservableCollection<ISportCardViewModel> ViewModels
        //    {
        //        get { return _viewModels; }
        //        set
        //        {
        //            if (value != null && value != _viewModels)
        //            {
        //                ReleaseViewModels(_viewModels);
        //                _viewModels = value;
        //                RaisePropertyChanged(() => ViewModels);
        //            }
        //        }
        //    }

        //    void ReleaseViewModels(IEnumerable<ISportCardViewModel> models)
        //    {
        //        if (models == null)
        //            return;
        //        foreach (ISportCardViewModel model in models)
        //        {
        //            ReleaseViewModel(model);
        //        }
        //    }

        //    void ReleaseViewModel(ISportCardViewModel model)
        //    {
        //        //Remove Handlers
        //    }

        //    #region ISportsPanelViewModel Members

        //    public ICommand NextPage
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    public ICommand PreviousPage
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    #endregion
        //}
    }
}
