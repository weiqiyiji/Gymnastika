using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanItemViewModel : NotificationObject, ISportsPlanItemViewModel
    {

        public SportsPlanItemViewModel(SportsPlanItem item)
        {
            Item = item;
            CloseCommand = new DelegateCommand(Close, CanClose);
        }

        public SportsPlanItemViewModel()
        {
            Item = new SportsPlanItem();
        }

        public event EventHandler CloseViewRequest =  delegate { };

        ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand;
            }
            set
            {
                if (_closeCommand != value)
                {
                    _closeCommand = value;
                    RaisePropertyChanged(() => CloseCommand);
                }
            }
        }

        SportsPlanItem _item;
        public SportsPlanItem Item
        {
            get
            {
                return _item;
            }
            set
            {
                if (value != null && _item != value)
                {
                    _item = value;
                    RaisePropertyChanged(() => Item);
                }
            }
        }

        public Sport Sport
        {
            get
            {
                return Item.Sport;
            }
            set
            {
                if (Item.Sport != value)
                {
                    Item.Sport = value;
                    RaisePropertyChanged(() => Sport);
                }
            }
        }

        public DateTime Time
        {
            get
            {
                return Item.Time;
            }
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
            get
            {
                return Item.Completed;
            }
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
            get
            {
                return Item.Duration;
            }
            set
            {
                if (value != Item.Duration)
                {
                    Item.Duration = value;
                    RaisePropertyChanged(() => Duration);
                }
            }
        }

        private void Close()
        {
            CloseViewRequest(this, EventArgs.Empty);
        }

        public bool CanClose() 
        {
            return true;
        }
    }
}
