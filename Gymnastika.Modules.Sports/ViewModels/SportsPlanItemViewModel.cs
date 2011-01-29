using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Interface;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    class SportsPlanItemViewModel : NotificationObject
    {
        ISportsPlanItem _model;
        public ISportsPlanItem Model 
        {
            get
            {
                return _model;
            }
            set
            {
                if(_model!=value)
                {
                    _model = value;
                    RaisePropertyChanged("Model");
                }
            }
        }
    
    }
}
