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
        void SportsPlanChanged(SportsPlan plan);

        DateTime Time { get; }

        string Date { get; }

        SportsPlan SportsPlan { get; set; }
    }

    public class SportsPlanViewModel : NotificationObject, ISportsPlanViewModel, IDropTarget
    {
        //IEventAggregator _aggregator;
        ISportsPlanItemViewModelFactory _factory;
        ISportsPlanProvider _planProvider;
        IPlanItemProvider _itemProvider;
        ISessionManager _sessionManager;

        public SportsPlanViewModel(ISportsPlanProvider planProvider,IPlanItemProvider itemProvider,ISessionManager sessionManager,ISportsPlanItemViewModelFactory factory)
        {
            _planProvider = planProvider;
            _itemProvider = itemProvider;

            _factory = factory;

            _sessionManager = sessionManager;

            SportsPlanItemViewModels.CollectionChanged += ItemsChanged;

            SportsPlan = CreateNewPlan();

            CanSave = Validate;

            _saveCommand = new DelegateCommand(Save, CanSave);
        }

        public string Date
        {
            get
            {
                return Time.ToString("yyyy/MM/dd");
            }
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
            Sport sport = null;

            if (dropInfo.Data is ISportCardViewModel)
            {
                sport = (dropInfo.Data as ISportCardViewModel).Sport;
            }

            if (dropInfo.Data is Sport)
            {
                sport = dropInfo.Data as Sport;
            }

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
            {
                ReleaseViewmodel(viewmodel);
            }
        }

        void OnItemSubmitRequest(object sender, EventArgs args)
        {
            ISportsPlanItemViewModel viewmodel = sender as ISportsPlanItemViewModel;
            var item = viewmodel.Item;
            using (_itemProvider.GetContextScope())
            {
                item.SportsPlanId = SportsPlan.Id;
                _itemProvider.CreateOrUpdate(item);
            }
        }

        private void ReleaseViewmodel(ISportsPlanItemViewModel viewmodel)
        {
            viewmodel.CancleRequest -= OnItemCancleRequest;
            viewmodel.PropertyChanged -= ItemPropertyChanged;
            viewmodel.SubmitRequest -= OnItemSubmitRequest;
            SportsPlanItemViewModels.Remove(viewmodel);
            var item = viewmodel.Item;
            if (item.Id != 0)
            {
                _itemProvider.Delete(item);
                item.Id = 0;
            }
        }

        private ISportsPlanItemViewModel CreateViewmodel(SportsPlanItem item)
        {
            var viewmodel = _factory.Create(item);
            viewmodel.Item.Duration = 30;
            viewmodel.CancleRequest += OnItemCancleRequest;
            viewmodel.PropertyChanged += ItemPropertyChanged;
            viewmodel.SubmitRequest += OnItemSubmitRequest;
            return viewmodel;
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
            ISportsPlanItemViewModel viewmodel = CreateViewmodel(item);
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
            //Notify the Save Button
            RaisePropertyChanged(() => CanSave);
        }

        void AddItem(SportsPlanItem item)
        {
            SportsPlan.SportsPlanItems.Add(item);
        }

        void DeleteItem(SportsPlanItem item)
        {
            SportsPlan.SportsPlanItems.Remove(item);
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
                        AddItem(plan.Item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ISportsPlanItemViewModel plan in oldPlans)
                    {
                        DeleteItem(plan.Item);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        SportsPlan CreateNewPlan()
        {
            SportsPlan plan = new SportsPlan();

            plan.User = _sessionManager.GetCurrentSession().AssociatedUser;
            
            using (_planProvider.GetContextScope())
            {
                _planProvider.Create(plan);
            }
            return plan;
        }

        void Save()
        {
            foreach (var model in SportsPlanItemViewModels)
            {
                if (model.SubmitCommand.CanExecute(null))
                {
                    model.SubmitCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("{0}信息错误", model.SportName);
                    return;
                }
            }
            using (_planProvider.GetContextScope())
            {
                _planProvider.CreateOrUpdate(SportsPlan);
            }
        }

        ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand; }
        }


        bool Validate()
        {
            foreach (var model in SportsPlanItemViewModels)
            {
                if (!model.SubmitCommand.CanExecute(null))
                    return false;
            }
            //这里是关于Plan的验证
            //
            return true;
        }

        Func<bool> _canSave;
        public Func<bool> CanSave 
        {
            get { return _canSave; }
            set
            {
                if (value != _canSave)
                {
                    _canSave = value;
                    RaisePropertyChanged(() => CanSave);
                }

            }
        }
    }
}
