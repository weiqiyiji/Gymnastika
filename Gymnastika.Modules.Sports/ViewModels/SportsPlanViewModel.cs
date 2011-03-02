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
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Data;
using System.ComponentModel;
using Gymnastika.Common.Extensions;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanViewModel : NotificationObject , ISportsPlanViewModel
    {
        IEventAggregator _aggregator;
        

        public SportsPlanViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            aggregator.GetEvent<SportsPlanChangedEvent>().Subscribe(SportsPlanChanged);

            SortDescriptions.Add(new SortDescription("SportsTime", ListSortDirection.Ascending));
        }

        public ICollectionView View
        {
            get
            {
                return CollectionViewSource.GetDefaultView(this.SportsPlanItems);
            }
        }

        SortDescriptionCollection _sortDescriptions;
        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                return _sortDescriptions;
            }
            set
            {
                if (_sortDescriptions != value)
                {
                    _sortDescriptions = value;
                    View.SortDescriptions.Clear();
                    View.SortDescriptions.AddRange(value);
                    RaisePropertyChanged(() => SortDescriptions);
                }
            }
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
                    //RaisePropertyChanged(() => SportsPlanItems);
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
                    SportsPlan.SportsPlanItems = value;
                    View.SortDescriptions.Clear();
                    View.SortDescriptions.AddRange(SortDescriptions);
                    RaisePropertyChanged(() => SportsPlanItems);
                }
            }
        }
    }
}
