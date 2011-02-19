using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportViewModel : NotificationObject, ISportViewModel, IDropTarget, IDragSource
    {
        Sport _sport;
        public Sport Sport
        {
            get { return _sport; }
            set
            {
                if (_sport != value)
                {
                    _sport = value;
                    RaisePropertyChanged("Sport");
                }
            }
        }

        #region IDropTarget Members

        public void DragOver(DropInfo dropInfo)
        {

            if (dropInfo.Data is Sport || dropInfo.Data is SportsPlanItem)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.All;
            }

        }

        public void Drop(DropInfo dropInfo)
        {
            if (dropInfo.Data is Sport)
                this.Sport = dropInfo.Data as Sport;
            else if (dropInfo.Data is SportsPlanItem)
                this.Sport = (dropInfo.Data as SportsPlanItem).Sport;
        }

        #endregion

        #region IDragSource Members

        public void StartDrag(DragInfo dragInfo)
        {
            dragInfo.Effects = DragDropEffects.All;
            dragInfo.Data = this.Sport;
        }

        #endregion
    }
}
