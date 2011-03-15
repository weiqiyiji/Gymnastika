using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Models;
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
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Modules.Sports.Services.Factories;
using Gymnastika.Services.Session;

namespace Gymnastika.Modules.Sports.ViewModels
{

    public interface ISportsPlanViewModel
    {

        event EventHandler RequestCancelEvent;

        event EventHandler RequestSubmitEvent;

        event EventHandler RequestDeleteEvent;

        DelegateCommand SubmitCommand { get; }

        DelegateCommand DeleteCommand { get; }

        DelegateCommand CancelCommand { get; }

        DateTime Time { get; }

        string Date { get; }

        SportsPlan SportsPlan { get; }
    }

    public class SportsPlanViewModel : NotificationObject, ISportsPlanViewModel, IDropTarget
    {
        ISportsPlanItemViewModelFactory _factory;
        ISportsPlanProvider _planProvider;
        IPlanItemProvider _itemProvider;
        ISportProvider _sportProvider;
        public SportsPlanViewModel(SportsPlan plan,ISportProvider sportProvider,ISportsPlanProvider planProvider,IPlanItemProvider itemProvider, ISportsPlanItemViewModelFactory factory)
        {
            _sportProvider = sportProvider;
            _planProvider = planProvider;
            _itemProvider = itemProvider;
            _factory = factory;
            SportsPlanItemViewModels.CollectionChanged += ItemsChanged;
            SportsPlan = plan;
        }

        private IList<SportsPlanItem> RemoveBuffer = new List<SportsPlanItem>();
        private IList<SportsPlanItem> ItemsBuffer = new List<SportsPlanItem>();

        public string Date
        {
            get { return Time.ToString("yyyy年MM月dd日"); }
        }

        public DateTime Time
        {
            get { return SportsPlan.Time; }
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

        int ToRange(int min, int max, int value)
        {
            return Math.Min(Math.Max(value, min), max);
        }


        SportsPlan _sportsPlan;
        public SportsPlan SportsPlan
        {
            get { return _sportsPlan; }
            private set
            {
                if (value != null && value != _sportsPlan)
                {
                    _sportsPlan = value;
                    SportsPlanItemViewModels.ReplaceBy(InitViewModels(_sportsPlan));
                    RaisePropertyChanged(() => SportsPlan);
                }
            }
        }

       IList<ISportsPlanItemViewModel>  InitViewModels(SportsPlan plan)
       {
           IList<SportsPlanItem> items = null;
           using (_itemProvider.GetContextScope())
           {
               items = _itemProvider.Fetch((t) => t.SportsPlan.Id == plan.Id).ToList();
               foreach (var item in items)
                   item.Sport = _sportProvider.Get(item.Sport.Id);
           }
           return CreateViewmodels(items);
       }

       private IList<ISportsPlanItemViewModel> CreateViewmodels(IEnumerable<SportsPlanItem> items)
       {
           var viewmodels = new List<ISportsPlanItemViewModel>();
           foreach (var item in items)
           {
               viewmodels.Add(CreateViewmodel(item));
           }
           return viewmodels;
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
            Sport sport = null;

            if (dropInfo.Data is ISportCardViewModel)
                sport = (dropInfo.Data as ISportCardViewModel).Sport;
            else if (dropInfo.Data is Sport)
                sport = dropInfo.Data as Sport;

            if (sport != null)
            {
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            }
        }

        void OnItemCancleRequest(object sender, EventArgs args)
        {
            ISportsPlanItemViewModel viewmodel = sender as ISportsPlanItemViewModel;
            if (viewmodel != null)
                SportsPlanItemViewModels.Remove(viewmodel);
        }

        private void ReleaseViewmodel(ISportsPlanItemViewModel viewmodel)
        {
            viewmodel.RequestCancleEvent -= OnItemCancleRequest;
            viewmodel.PropertyChanged -= ItemPropertyChanged;
            //SportsPlanItemViewModels.Remove(viewmodel);
        }

        private ISportsPlanItemViewModel CreateViewmodel(SportsPlanItem item)
        {
            var viewmodel = _factory.Create(item);
            viewmodel.Item.Duration = 30;
            viewmodel.RequestCancleEvent += OnItemCancleRequest;
            viewmodel.PropertyChanged += ItemPropertyChanged;
            return viewmodel;
        }

        void UpdateCalories()
        {
            TotalCalories = GetTotalCalories();
        }

        double GetTotalCalories()
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
            ISportsPlanItemViewModel viewmodel = CreateViewmodel(item);
            SportsPlanItemViewModels.Add(viewmodel);
        }



        #endregion

        public void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ISportsPlanItemViewModel viewmodel = sender as ISportsPlanItemViewModel;
            if (viewmodel != null)
            {
                UpdateState();
            }
        }

        void UpdateState()
        {
            //Notify the energe bar
            UpdateCalories();
            SubmitCommand.RaiseCanExecuteChanged();
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
                        OnAddModel(plan);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ISportsPlanItemViewModel plan in oldPlans)
                    {
                        OnDeleteModel(plan);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnDeleteModel(ISportsPlanItemViewModel sportsPlanItem)
        {
            ReleaseViewmodel(sportsPlanItem);
            SportsPlanItem item = sportsPlanItem.Item;
            ItemsBuffer.Remove(item);
            if (item.Id != 0)
                RemoveBuffer.Add(item);
        }

        private void OnAddModel(ISportsPlanItemViewModel sportsPlanItem)
        {
            SportsPlanItem item = sportsPlanItem.Item;
            ItemsBuffer.Add(item);
        }


        void DeleteItemFromRepository(SportsPlanItem item)
        {
            _itemProvider.Delete(item);
            item.Id = 0;
        }

        void UpdateOrCreateItemToRepository(SportsPlanItem item)
        {
            _itemProvider.CreateOrUpdate(item);
        }
        void UpdatePlan()
        {
            SportsPlan.SportsPlanItems = ItemsBuffer;

            using (_itemProvider.GetContextScope())
            {
                foreach (var item in RemoveBuffer)
                {
                    DeleteItemFromRepository(item);
                }
                foreach (var item in SportsPlan.SportsPlanItems)
                {
                    UpdateOrCreateItemToRepository(item);
                }
            }
            RemoveBuffer.Clear();
        }

        void Sumbmit()
        {
            UpdatePlan();

            using (_planProvider.GetContextScope())
            {
                _planProvider.CreateOrUpdate(SportsPlan);
            }

            if (RequestSubmitEvent != null)
                RequestSubmitEvent(this, EventArgs.Empty);
        }

        DelegateCommand _submitCommand;
        public DelegateCommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                    _submitCommand = new DelegateCommand(Sumbmit, CanSubmit);
                return _submitCommand;
            }
        }


        bool CanSubmit()
        {
            return true;
        }

        public event EventHandler RequestCancelEvent = delegate { };

        public event EventHandler RequestSubmitEvent = delegate { };


        DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new DelegateCommand(Cancel);
                return _cancelCommand;
            }
        }


        void Cancel()
        {
            if (RequestCancelEvent == null)
                RequestCancelEvent(this, EventArgs.Empty);
        }



        public event EventHandler RequestDeleteEvent = delegate { };

        DelegateCommand _deleteCommand;
        public DelegateCommand DeleteCommand
        {
            get 
            {
                if(_deleteCommand==null)
                    _deleteCommand = new DelegateCommand(Delete);
                return _deleteCommand;
            }
        }


        void Delete()
        {
            using (_itemProvider.GetContextScope())
            {
                DeletePlan();
            }
            if (RequestDeleteEvent != null)
                RequestDeleteEvent(this, EventArgs.Empty);
        }

        private void DeletePlan()
        {
            RemoveBuffer.Clear();
            ItemsBuffer.Clear();
            foreach (var item in SportsPlan.SportsPlanItems)
            {
                _itemProvider.Delete(item);
                SportsPlan.SportsPlanItems.Remove(item);
            }
            _planProvider.Delete(SportsPlan);
        }

        
    }
}
