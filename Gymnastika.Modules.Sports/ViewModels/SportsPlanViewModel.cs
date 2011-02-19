using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Models;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Sports.Events;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanViewModel :NotificationObject, ISportsPlanViewModel , IDropTarget , IDragSource
    {
        IUnityContainer _container;

        public SportsPlanViewModel(IUnityContainer container)
        {
            this._container = container;
        }

        #region ISportsPlanViewModel Members

        SportsPlan _sportsPlan;
        public Models.SportsPlan SportsPlan
        {
            get
            {
                return _sportsPlan;
            }
            set
            {
                if (_sportsPlan != value)
                {
                    _sportsPlan = value;
                    RaisePropertyChanged("SportsPlan");
                }
            }
        }


        #endregion


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
            SportsPlanItem item =  new SportsPlanItem() { Sport = sourceItem };
            if (target == null)
            {
                this.SportsPlan.SportsPlanItems.Add(item);
            }
            else
            {
                this.SportsPlan.SportsPlanItems.Insert(dropInfo.InsertIndex, item);
            }
        }

        #endregion

        SportsPlanItem _selectedPlanItem;
        public SportsPlanItem SelectedPlanItem
        {
            get
            { 
                return _selectedPlanItem; 
            }
            set
            {
                if (_selectedPlanItem != value)
                {
                    _selectedPlanItem = value;
                    RaisePropertyChanged("SelectedPlanItem");
                }
            }
        }

        private void OnCancel(SportsPlanItem item)
        {
            this.SportsPlan.SportsPlanItems.Remove(item);
        }


        ICommand _cancleCommand = null;
        public ICommand CancleCommand
        {
            get
            {
                if (_cancleCommand == null)
                {
                    _cancleCommand = new DelegateCommand<SportsPlanItem>(OnCancel);
                }
                return _cancleCommand;
            }
        }

        #region IDragSource Members

        public void StartDrag(DragInfo dragInfo)
        {
            dragInfo.Data = SelectedPlanItem;
            dragInfo.Effects = DragDropEffects.All;
        }

        #endregion


        ICommand _showPlanDetail = null;
        public ICommand ShowPlanDetail
        {
            get
            {
                if (_showPlanDetail == null)
                {
                    _showPlanDetail = new DelegateCommand(ShowSportsPlanDetail, CanShowSportsPlanDetail);
                }
                return _showPlanDetail; 
            }
            
        }

        bool CanShowSportsPlanDetail()
        {
            return this.SportsPlan.SportsPlanItems.Count() != 0;
        }

        void ShowSportsPlanDetail()
        {
            _container.Resolve<ShowPlanDetailEvent>().Publish(this.SportsPlan);
        }


    }
}
