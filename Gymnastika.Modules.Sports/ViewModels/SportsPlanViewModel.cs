using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanViewModel :NotificationObject, ISportsPlanViewModel
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
    }
}
