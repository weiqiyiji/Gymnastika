using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Interface;

namespace Gymnastika.Modules.Sports.Models
{

    public class SportsPlanItem : NotificationObject, ISportsPlanItem
    {

        #region ISportsPlanItem Members

        ISport _sport;
        public ISport Sport 
        {
            get
            {
                return _sport;
            }
            set
            {
                if (value != _sport)
                {
                    _sport = value;
                    RaisePropertyChanged("Sport");
                }
            }
        }

        int _hour;
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                if (_hour != value)
                {
                    _hour = value;
                    RaisePropertyChanged("Hour");
                }
            }
        }

        int _min;
        public int Min
        {
            get
            {
                return _min;
            }
            set
            {
                if (_min != value)
                {
                    _min = value;
                    RaisePropertyChanged("Min");
                }
            }
        }

        int _duration;
        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    RaisePropertyChanged("Duration");
                }
            }
        }

        bool _hasCompleted;
        public bool HasCompleted
        {
            get
            {
                return _hasCompleted;
            }
            set
            {
                if (_hasCompleted != value)
                {
                    _hasCompleted = value;
                    RaisePropertyChanged("HasCompleted");
                }
            }
        }

        int _score;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    RaisePropertyChanged("Score");
                }
            }
        }

        #endregion
    }
}
