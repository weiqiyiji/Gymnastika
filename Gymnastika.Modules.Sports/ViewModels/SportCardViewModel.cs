using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public class SportCardViewModel : NotificationObject, ISportCardViewModel, IDragSource
    {

        public SportCardViewModel(Models.Sport sport)
        {
            Sport = sport;
        }
        

        Sport _sport;
        public Sport Sport
        {
            get
            {
                return _sport;
            }
            set
            {
                if (_sport != value)
                {
                    _sport = value;
                    RaisePropertyChanged(() => Sport);
                }
            }
        }

        public String Name
        {
            get
            {
                return Sport.Name;
            }
            set
            {
                if (Sport.Name != value)
                {
                    Sport.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            
            }
        }

        public string IntroductionUri
        {
            get
            {
                return Sport.IntroductionUri;
            }
            set
            {
                if (Sport.IntroductionUri != value)
                {
                    Sport.IntroductionUri = value;
                    RaisePropertyChanged(() => IntroductionUri);
                }
            }
        }
        public String ImageUri
        {
            get
            {
                return Sport.ImageUri;
            }
            set
            {
                if (Sport.ImageUri != value)
                {
                    Sport.ImageUri = value;
                    RaisePropertyChanged(() => ImageUri);
                }
            }
        }

        public int CaloriePerHour 
        {
            get { return Sport.CaloriePerHour; }

            set
            {
                if (Sport.CaloriePerHour != value)
                {
                    Sport.CaloriePerHour = value;
                    RaisePropertyChanged(() => CaloriePerHour);
                }
            }
        }
        public String Brief
        {
            get { return Sport.Brief; }
            set
            {
                if (Sport.Brief != value)
                {
                    Sport.Brief = value;
                    RaisePropertyChanged(()=>Brief);
                }
            }
        }
        

        #region IDragSource Members

        public void StartDrag(DragInfo dragInfo)
        {
            dragInfo.Data = Sport;
            dragInfo.Effects = DragDropEffects.All;
        }

        #endregion
    }
}
