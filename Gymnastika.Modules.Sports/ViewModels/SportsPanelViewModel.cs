using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Events;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Extensions;
using GongSolutions.Wpf.DragDrop;
using Gymnastika.Modules.Sports.Services;
using System.Windows.Data;
using System.ComponentModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPanelViewModel : NotificationObject, ISportsPanelViewModel
    {
        IEventAggregator _aggregator;
        ISportCardViewModelFactory _factory;
        
        public SportsPanelViewModel(IEventAggregator aggregator,ISportCardViewModelFactory factory)
        {
            _aggregator = aggregator;
            _factory = factory;
            _aggregator.GetEvent<CategoryChangedEvent>().Subscribe(CategoryChanged);
        

        }

        public ICollectionView View
        {
            get { return CollectionViewSource.GetDefaultView(ViewModels); }
        }

        Predicate<ISportCardViewModel> _filter;
        Predicate<ISportCardViewModel> Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    RaisePropertyChanged(() => Filter);
                    View.Filter = (Predicate<object>)_filter;
                    View.Refresh();
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
                }
            }
        }

        public void CategoryChanged(SportsCategory category)
        {
            Category = category;
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
                    Sports = _category.Sports;
                    RaisePropertyChanged(() => Category);
                }
            }
        }

        private IList<Sport> _sports;

        public IList<Sport> Sports
        {
            get { return _sports; }
            set
            {
                if (_sports != value)
                {
                    _sports = value;
                    ViewModels = GetViewModels(_sports);
                    RaisePropertyChanged(() => Sports);
                }
            }
        }

        private ObservableCollection<ISportCardViewModel> GetViewModels(IList<Sport> sports)
        {
            ObservableCollection<ISportCardViewModel> viewModels = new ObservableCollection<ISportCardViewModel>();
            foreach (Sport sport in sports)
            {
                ISportCardViewModel viewmodel = _factory.Create(sport);
                viewModels.Add(viewmodel);
            }
            return viewModels;
        }
        

        ObservableCollection<ISportCardViewModel> _viewModels = new ObservableCollection<ISportCardViewModel>();
        public ObservableCollection<ISportCardViewModel> ViewModels
        {
            get { return _viewModels; }
            set
            {
                if (value != null && value != _viewModels)
                {
                    _viewModels = value;
                    RaisePropertyChanged(() => ViewModels);
                }
            }
        }

    }
}
