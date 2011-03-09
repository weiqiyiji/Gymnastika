using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Services;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Models;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Extensions;
using Gymnastika.Modules.Sports.Services.Providers;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ICategoriesPanelViewModel
    {
        ObservableCollection<SportsCategory> Categories { get; set; }

        event EventHandler CategorySelectedEvent;
        
        SportsCategory CurrentSelectedItem { get; set; }
    }

    public class CategoriesPanelViewModel : NotificationObject, ICategoriesPanelViewModel
    {
        ICategoryProvider _provider;
        IEventAggregator _aggregator;

        public CategoriesPanelViewModel(ICategoryProvider provider,IEventAggregator aggregator)
        {
            _provider = provider;
            _aggregator = aggregator;

            using (_provider.GetContextScope())
            {
                _categories = _provider.All().ToObservableCollection();
            }

            CurrentSelectedItem = _categories.FirstOrDefault();
        }

        ObservableCollection<SportsCategory> _categories;
        public ObservableCollection<SportsCategory> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    RaisePropertyChanged("Categories");
                }
            }
        }

        SportsCategory _currentSelectedItem;
        public SportsCategory CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set
            {
                if (value != null && _currentSelectedItem != value)
                {
                    _currentSelectedItem = value;
                    RaisePropertyChanged(() => CurrentSelectedItem);
                    RaiseCategorySelectedEvent(_currentSelectedItem);
                }
            }
        }

        void RaiseCategorySelectedEvent(SportsCategory category)
        {
            if (CategorySelectedEvent != null)
                CategorySelectedEvent(this, EventArgs.Empty);
        }

        public event EventHandler CategorySelectedEvent;
    
    }
}