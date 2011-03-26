using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Events;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class AmountSelectorViewModel : NotificationObject
    {
        private readonly IEventAggregator _eventAggregator;

        public AmountSelectorViewModel(AmountSelectorView view, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _currentValue = 0;
            View = view;
            View.Model = this;
        }

        public AmountSelectorView View { get; set; }

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
                    _eventAggregator.GetEvent<AmountChangedEvent>().Publish(_currentValue);
                }
            }
        }
    }
}
