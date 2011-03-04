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
using System.Collections.Specialized;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanViewModel : NotificationObject, ISportsPlanViewModel, IDropTarget
    {
        IEventAggregator _aggregator;
        ISportsPlanItemViewModelFactory _factory;



        public SportsPlanViewModel(IEventAggregator aggregator, ISportsPlanItemViewModelFactory factory,SportsPlan plan)
        {
            _factory = factory;
            
            _aggregator = aggregator;
            
            aggregator.GetEvent<SportsPlanChangedEvent>().Subscribe(SportsPlanChanged);
            
            SportsPlanItemViewModels.CollectionChanged += ItemsChanged;
            
            SportsPlan = plan ?? new SportsPlan();

            SportsPlan.SportsPlanItems = SportsPlan.SportsPlanItems ?? new List<SportsPlanItem>();

        }

        public SportsPlanViewModel(IEventAggregator aggregator, ISportsPlanItemViewModelFactory factory)
        :this(aggregator, factory, new SportsPlan(){ SportsPlanItems = new List<SportsPlanItem>() })
        {

        }

        double? _totalCalories = 0;
        public double? TotalCalories
        {
            get
            {
                return _totalCalories;
            }
            set
            {
                if (_totalCalories != value)
                {
                    _totalCalories = value;
                    RaisePropertyChanged(() => TotalCalories);
                }
            }
        }

        public ICollectionView View
        {
            get
            {
                return CollectionViewSource.GetDefaultView(SportsPlanItemViewModels);
            }
        }

        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                return View.SortDescriptions;
            }
        }


        public void SportsPlanChanged(SportsPlan plan)
        {
            SportsPlan = plan;
        }

        SportsPlan _sportsPlan;
        public SportsPlan SportsPlan
        {
            get { return _sportsPlan; }
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

        void OnCloseRequest(object sender, EventArgs args)
        {
            ISportsPlanItemViewModel viewmodel = sender as ISportsPlanItemViewModel;
            if (viewmodel != null)
            {
                ReleaseItem(viewmodel);
            }
        }

        private void ReleaseItem(ISportsPlanItemViewModel viewmodel)
        {
            viewmodel.CloseViewRequest -= OnCloseRequest;
            viewmodel.PropertyChanged -= ItemPropertyChanged;
            SportsPlanItemViewModels.Remove(viewmodel);
        }

        void UpdateCalories()
        {
            TotalCalories = CaculateTotalCalories();
        }

        double CaculateTotalCalories()
        {
            double calories = 0;
            foreach (ISportsPlanItemViewModel viewmodel in SportsPlanItemViewModels)
            {
                SportsPlanItem item = viewmodel.Item;
                Sport sport = item.Sport;
                calories += sport.Calories * item.Duration / sport.Minutes;
            }
            return calories;
        }

        public void Drop(DropInfo dropInfo)
        {
            Sport sourceItem = dropInfo.Data as Sport;
            object target = dropInfo.TargetItem;
            SportsPlanItem item = new SportsPlanItem() { Sport = sourceItem };
            ISportsPlanItemViewModel viewmodel = CreateItem(item);
            if (target == null)
            {
                SportsPlanItemViewModels.Add(viewmodel);
            }
            else
            {
                SportsPlanItemViewModels.Insert(dropInfo.InsertIndex, viewmodel);
            }

        }

        private ISportsPlanItemViewModel CreateItem(SportsPlanItem item)
        {
            var viewmodel = _factory.Create(item);
            viewmodel.Item.Duration = 30;
            viewmodel.CloseViewRequest += OnCloseRequest;
            viewmodel.PropertyChanged += ItemPropertyChanged;
            return viewmodel;
        }

        #endregion

        public void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ISportsPlanItemViewModel viewmodel = sender as ISportsPlanItemViewModel;
            if (viewmodel != null)
            {
                UpdateCalories();
            }
        }


        public void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCalories();
            var newPlans = e.NewItems;
            var oldPlans = e.OldItems;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ISportsPlanItemViewModel plan in newPlans)
                    {
                        SportsPlan.SportsPlanItems.Add(plan.Item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ISportsPlanItemViewModel plan in oldPlans)
                    {
                        SportsPlan.SportsPlanItems.Remove(plan.Item);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
