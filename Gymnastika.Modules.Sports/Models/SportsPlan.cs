using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Interface;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlan :NotificationObject , ISportsPlan
    {
        #region ISportsPlan Members

        IList<ISportsPlanItem> _sportsPlanItems = new BindingList<ISportsPlanItem>();
        IList<ISportsPlanItem> ISportsPlan.SportsPlanItems
        {
            get
            {
                return _sportsPlanItems;
            }
            set
            {
                if (value != _sportsPlanItems)
                {
                    _sportsPlanItems = value;
                    RaisePropertyChanged("SportsPlanItems");
                }
            }
        }

        #endregion
    }
}
