using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportsPlanDetailViewModel : NotificationObject, ISportsPlanDetailViewModel
    {
        public SportsPlanDetailViewModel(SportsPlan sportsPlan)
        {
            _sportsPlan = sportsPlan;
            UpdateDatas();
        }


        SportsPlan _sportsPlan = null;

        public SportsPlan SportsPlan
        {
            get { return _sportsPlan; }
            set
            {
                if (_sportsPlan != value)
                {
                    _sportsPlan = value;
                    RaisePropertyChanged("SportsPlan");
                }
            }
        }

        int _totalCalories = 0;

        public int TotalCalories
        {
            get { return _totalCalories; }
            set
            {
                if (_totalCalories != value)
                {
                    _totalCalories = value;
                    RaisePropertyChanged("TotalCalories");
                }
            }
        }

        void UpdateCalories()
        {
            int totalColories = 0;
            foreach (var item in SportsPlan.SportsPlanItems)
            {
                totalColories += item.Duration * item.Sport.CaloriePerHour / 60;
            }
            this.TotalCalories = totalColories;
        }

        void UpdateDatas()
        {
            UpdateCalories();

        }
    
    }
}
