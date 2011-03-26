using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class AmountSelectorViewModel : NotificationObject
    {
        public AmountSelectorViewModel()
        {
            CurrentValue = 0;
        }

        private int _currentValue;
        public int CurrentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    RaisePropertyChanged("CurrentValue");
                }
            }
        }
    }
}
