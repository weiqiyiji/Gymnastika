using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Models;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Diagnostics;

namespace Gymnastika.Modules.Sports.Views
{
    public class SportsPlanViewModel :NotificationObject, ISportsPlanViewModel , IDropTarget
    {

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

        //private void DragEnter(object sender, DragEventArgs e)

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
    }
}
