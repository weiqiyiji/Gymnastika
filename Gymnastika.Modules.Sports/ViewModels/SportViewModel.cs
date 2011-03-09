using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportViewModel : INotifyPropertyChanged
    {
    }

    public class SportViewModel : NotificationObject , ISportViewModel
    {
        public SportViewModel()
        :this(new Sport())
        {

        }

        public SportViewModel(Sport sport)
        {
            Sport = sport;
        }

        private Sport _sport;
        public Sport Sport
        {
            get { return _sport; }
            set
            {
                if (_sport != value)
                {
                    _sport = value;
                    RaisePropertyChanged("");   
                }
            }
        }

        public string ImageUri
        {
            get { return Sport.ImageUri; }
        }

        public string Brief
        {
            get { return Sport.Brief; }
        }


        public string Name
        {
            get{return Sport.Name;}
        }

        public double Calories
        {
            get { return Sport.Calories; }
        }

        public int Minutes
        {
            get { return Sport.Minutes; }
        }

        public string IntroductionUri
        {
            get { return Sport.IntroductionUri; }
        }
        
    }
}
