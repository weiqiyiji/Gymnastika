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
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using Gymnastika.Modules.Sports.Services;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanViewModel : NotificationObject , ISportsPlanViewModel , IDropTarget
    {
        IEventAggregator _aggregator;
        ISportsPlanItemViewModelFactory _factory;

        public SportsPlanViewModel(IEventAggregator aggregator,ISportsPlanItemViewModelFactory factory)
        {
            _factory = factory;
            _aggregator = aggregator;
            aggregator.GetEvent<SportsPlanChangedEvent>().Subscribe(SportsPlanChanged);
        }


        public ICollectionView View
        {
            get
            {
                return CollectionViewSource.GetDefaultView(SportsPlanItemViewModels);
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
                }
            }
        }



        ObservableCollection<ISportsPlanItemViewModel> _sportsPlanItemViewModels = new ObservableCollection<ISportsPlanItemViewModel>();
        public ObservableCollection<ISportsPlanItemViewModel> SportsPlanItemViewModels
        {
            get
            {
                return _sportsPlanItemViewModels;
            }
        }

        #region IDropTarget Members

        public void DragOver(DropInfo dropInfo)
        {
            if (dropInfo.Data is Sport)
            {
                dropInfo.Effects = DragDropEffects.Copy;

                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            }
        }

        public void Drop(DropInfo dropInfo)
        {
            Sport sourceItem = dropInfo.Data as Sport;
            object target = dropInfo.TargetItem;
            SportsPlanItem item = new SportsPlanItem() { Sport = sourceItem };
            ISportsPlanItemViewModel viewmodel =  _factory.Create(item);
            if (target == null)
            {
                SportsPlanItemViewModels.Add(viewmodel);
            }
            else
            {
                SportsPlanItemViewModels.Insert(dropInfo.InsertIndex, viewmodel);
            }
            
        }

        #endregion
    }
}
