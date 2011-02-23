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

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class CategoriesPanelViewModel : NotificationObject, ICategoriesPanelViewModel
    {
        ICategoriesProvider _provider;
        IEventAggregator _aggregator;

        public CategoriesPanelViewModel(ICategoriesProvider provider,IEventAggregator aggregator)
        {
            _provider = provider;
            _aggregator = aggregator;
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

        
    }
}
