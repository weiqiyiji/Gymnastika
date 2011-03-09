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
        event EventHandler CancleRequest;

        event EventHandler SubmitRequest;

        ICommand SubmitCommand { get; }

        ICommand CancleCommand { get; }

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
            CancleCommand = new DelegateCommand(Cancle, CanCancle);
            SubmitCommand = new DelegateCommand(Submit, CanSubmit);
        }

        public event EventHandler SubmitRequest = delegate { };

        public event EventHandler CancleRequest =  delegate { };


        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get { return _submitCommand; }
            set
            {
                if (value != _submitCommand)
                {
                    _submitCommand = value;
                    RaisePropertyChanged(() => SubmitCommand);
                }
            }
        }
        

        ICommand _cancleCommand;
        public ICommand CancleCommand
        {
            get { return _cancleCommand; }
            set
            {
                if (value != _cancleCommand)
                {
                    _cancleCommand = value;
                    RaisePropertyChanged(() => CancleCommand);
                }
            }
        }

        public string ImageUri
        {
            get { return Sport.ImageUri; }
        }

        SportsPlanItem _item;
        public SportsPlanItem Item
        {
            get { return _item; }
            set
            {
                if (value != null && _item != value)
                {
                    _item = value;
                    RaisePropertyChanged(() => Item);
                }
            }
        }

        public String SportName
        {
            get { return Sport.Name; }
        }

        public Sport Sport
        {
            get { return Item.Sport; }
            set
            {
                if (Item.Sport != value)
                {
                    Item.Sport = value;
                    RaisePropertyChanged(() => Sport);
                    RaisePropertyChanged(() => SportName);
                }
            }
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
            CancleRequest(this, EventArgs.Empty);
        }

        public bool CanCancle() 
        {
            return CancleRequest != null;
        }

        void Submit()
        {
            SubmitRequest(this, EventArgs.Empty);
        }

        bool CanSubmit()
        {
            return SubmitRequest != null;
        }


    }
}
