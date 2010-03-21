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
using Gymnastika.Modules.Sports.Facilities;
using Gymnastika.Services.Models;
using Gymnastika.Modules.Sports.Events;

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

        DateTime DateTime { get; }

        string Date { get; }

        string DayOfWeekDes { get; }

        int DayOfWeek { get; }

        User User { get; }

        SportsPlan SportsPlan { get; }

        IList<SportsPlanItem> ItemsBuffer { get; }

        IList<SportsPlanItem> RemoveBuffer { get; }

        bool SetPlan(DateTime date);
    }

    public class SportsPlanViewModel : NotificationObject, ISportsPlanViewModel, IDropTarget
    {

        ISportsPlanItemViewModelFactory _factory;
        ISportsPlanProvider _planProvider;
        IPlanItemProvider _itemProvider;
        ISportProvider _sportProvider;
        ISessionManager _sessionManager;
        IEventAggregator _eventAggregator;

        public SportsPlanViewModel(SportsPlan plan, ISessionManager sessionManager, ISportProvider sportProvider, ISportsPlanProvider planProvider, IPlanItemProvider itemProvider, ISportsPlanItemViewModelFactory factory,IEventAggregator eventAggregator)
        {
            _sportProvider = sportProvider;
            _planProvider = planProvider;
            _itemProvider = itemProvider;
            _factory = factory;
            _sessionManager = sessionManager;
            SportsPlanItemViewModels.CollectionChanged += ItemsChanged;
            SportsPlan = plan;
            _eventAggregator = eventAggregator;
        }

        public bool SetPlan(DateTime date)
        {
            SportsPlan plan = null;
            using (_planProvider.GetContextScope())
            {
                plan = _planProvider.FetchFirstOrDefault(date);
                if (plan != null)
                {
                    plan.SportsPlanItems = _itemProvider.All().ToList();
                    foreach (var item in plan.SportsPlanItems)
                        item.Sport = _sportProvider.Get(item.Sport.Id);
                }
            }
            if (plan != null)
            {
                this.SportsPlan = plan;
                return true;
            }
            else
            {
                this.SportsPlan = new SportsPlan() { User = this.User };
                return false;
            }
        }

        public DateTime DateTime
        {
            get { return new DateTime(SportsPlan.Year, SportsPlan.Month, SportsPlan.Day); }
        }

        IList<SportsPlanItem> _removeBuffer = new List<SportsPlanItem>();
        public IList<SportsPlanItem> RemoveBuffer
        {
            get { return _removeBuffer; }
        }

        IList<SportsPlanItem> _itemsBuffer = new List<SportsPlanItem>();
        public IList<SportsPlanItem> ItemsBuffer 
        {
            get { return _itemsBuffer; }
        }

        public int DayOfWeek
        {
            get { return (int)DateTime.DayOfWeek; }
        }

        public string DayOfWeekDes
        {
            get
            {
               return DateFacility.GetDayName((DayOfWeek)DayOfWeek);
            }
        }

        public string Date
        {
            get { return DateFacility.GetShortDate(DateTime); }
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
                    ResetPlan(_sportsPlan);
                    RaisePropertyChanged(() => SportsPlan);
                }
            }
        }

        void ResetPlan(SportsPlan plan)
        {
            ItemsBuffer.Clear();
            SportsPlanItemViewModels.ReplaceBy(InitViewModels(_sportsPlan));
            RemoveBuffer.Clear();
        }

       IList<ISportsPlanItemViewModel>  InitViewModels(SportsPlan plan)
       {
           IList<SportsPlanItem> items = plan.SportsPlanItems;
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

        public User User
        {
            get { return _sessionManager.GetCurrentSession().AssociatedUser; }
        }

        void SavePlanToRepository()
        {
            using (_planProvider.GetContextScope())
            {
                foreach (SportsPlanItem item in SportsPlan.SportsPlanItems)
                {
                    if (item.Id != 0)
                    {
                        _itemProvider.Delete(item);
                        item.Id = 0;
                    }
                }
                if (SportsPlan.Id == 0)
                {
                    SportsPlan.SportsPlanItems = new List<SportsPlanItem>();
                    SportsPlan.User = User;
                    _planProvider.Create(SportsPlan);
                }

                foreach (var item in ItemsBuffer)
                {
                    item.SportsPlan = SportsPlan;
                    _itemProvider.CreateOrUpdate(item);
                }

                SportsPlan.SportsPlanItems.ReplaceBy(ItemsBuffer);
                
                _planProvider.CreateOrUpdate(SportsPlan);

            }
            _eventAggregator.GetEvent<SportsPlanCreatedOrModifiedEvent>().Publish(SportsPlan);
        }

        void Sumbmit()
        {
            SavePlanToRepository();
            

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
            ResetPlan(SportsPlan);

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
                DeletePlanFromRepository();
            }
            if (RequestDeleteEvent != null)
                RequestDeleteEvent(this, EventArgs.Empty);
        }

        private void DeletePlanFromRepository()
        {
            if (SportsPlan.Id == 0)
                return;
            RemoveBuffer.Clear();
            ItemsBuffer.Clear();
            foreach (var item in SportsPlan.SportsPlanItems)
            {
                _itemProvider.Delete(item);
                SportsPlan.SportsPlanItems.Remove(item);
                item.Id = 0;
            }
            _planProvider.Delete(SportsPlan);
            SportsPlan.Id = 0;
        }

        
    }
}
