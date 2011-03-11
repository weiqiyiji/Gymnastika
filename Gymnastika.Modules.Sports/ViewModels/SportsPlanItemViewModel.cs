using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using Gymnastika.Modules.Sports.Services.Providers;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsPlanItemViewModel : INotifyPropertyChanged
    {
        event EventHandler RequestCancleEvent;

        DelegateCommand CancelCommand { get; }

        SportsPlanItem Item { get; }

        string ImageUri { get; }

        String SportName { get; }

        DateTime Time { get; }

        bool Completed { get; }

        int Duration { get; }
    }

    public class SportsPlanItemViewModel : NotificationObject, ISportsPlanItemViewModel
    {
        public SportsPlanItemViewModel(SportsPlanItem item)
        {
            Item = item;
        }

        public event EventHandler RequestCancleEvent =  delegate { };

        DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new DelegateCommand(Cancle);
                return _cancelCommand;
            }
        }

        public string ImageUri
        {
            get { return Sport.ImageUri; }
        }

        public SportsPlanItem Item
        {
            get;
            private set;
        }

        public String SportName
        {
            get { return Sport.Name; }
        }

        public Sport Sport
        {
            get { return Item.Sport; }
        }

        public DateTime Time
        {
            get { return Item.Time; }
            set
            {
                if (value != Item.Time)
                {
                    Item.Time = value;
                    RaisePropertyChanged(() => Time);
                }
            }
        }

        public bool Completed
        {
            get { return Item.Completed; }
            set
            {
                if (Item.Completed != value)
                {
                    Item.Completed = value;
                    RaisePropertyChanged(() => Completed);
                }
            }
        }

        public int Duration
        {
            get { return Item.Duration; }
            set
            {
                if (value != Item.Duration)
                {
                    Item.Duration = value;
                    RaisePropertyChanged(() => Duration);
                }
            }
        }

        private void Cancle()
        {
            if(RequestCancleEvent!=null)
                RequestCancleEvent(this, EventArgs.Empty);
        }


    }
}
