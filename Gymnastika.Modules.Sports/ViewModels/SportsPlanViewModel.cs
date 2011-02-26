using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.Events;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Extensions;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanViewModel : NotificationObject , ISportsPlanViewModel
    {
        IEventAggregator _aggregator;
        

        public SportsPlanViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            aggregator.GetEvent<SportsPlanChangedEvent>().Subscribe(SportsPlanChanged);
        }

        public void SportsPlanChanged(SportsPlan plan)
        {
           SportsPlan = plan; 
        }

        SportsPlan _sportsPlan;
        public SportsPlan SportsPlan
        {
            get{return _sportsPlan;}
            set
            {
                if (value != null && value != _sportsPlan)
                {
                    _sportsPlan = value;
                    RaisePropertyChanged(() => SportsPlan);
                    SportsPlanItems = _sportsPlan.SportsPlanItems.ToObservableCollection();
                }
            }
        }

                
        ObservableCollection<SportsPlanItem> _sportsPlanItems;
        ObservableCollection<SportsPlanItem> SportsPlanItems
        {
            get { return _sportsPlanItems; }
            set
            {
                if (_sportsPlanItems != value)
                {
                    _sportsPlanItems = value;
                    RaisePropertyChanged(() => SportsPlanItems);
                }
            }
        }
    }
}
