using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanItemViewModel : NotificationObject, ISportsPlanItemViewModel
    {

        public SportsPlanItemViewModel(SportsPlanItem item)
        {
            Item = item;
        }

        public SportsPlanItemViewModel()
        {
            Item = new SportsPlanItem();
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

        public DateTime SportsTime
        {
            get
            {
                return Item.SportsTime;
            }
            set
            {
                if (value != Item.SportsTime)
                {
                    Item.SportsTime = value;
                    RaisePropertyChanged(() => SportsTime);
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
    }
}
