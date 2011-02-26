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

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPanelViewModel : NotificationObject, ISportsPanelViewModel
    {
        IEventAggregator _aggregator;

        public SportsPanelViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.GetEvent<CategoryChangedEvent>().Subscribe(CategoryChanged);
        }

        public void CategoryChanged(SportsCategory category)
        {
            this.Sports = category.Sports.ToObservableCollection();
        }

        ObservableCollection<Sport> _sports;
        public ObservableCollection<Sport> Sports
        {
            get { return _sports; }
            set
            {
                if (value != null && value != _sports)
                {
                    _sports = value;
                    RaisePropertyChanged(() => Sports);
                }
            }
        }


    }
}
