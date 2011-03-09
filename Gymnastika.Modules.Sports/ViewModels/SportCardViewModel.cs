using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportCardViewModel
    {
        String Name { get; }
        double Calories { get; }
        int Minutes { get; }
    }

    public class SportCardViewModel : NotificationObject, ISportCardViewModel, IDragSource
    {
        public override string ToString()
        {
            return Name;
        }

        public SportCardViewModel(Sport sport)
        {
            Sport = sport;
            _showDetailCommand = new DelegateCommand(ShowDetail);
        }

        public event EventHandler RequestShowDetail = delegate { };

        void ShowDetail()
        {
            RequestShowDetail(this, EventArgs.Empty);
        }


        #region Command

        ICommand _showDetailCommand;
        public ICommand ShowDetailCommand
        {
            get
            {
                return _showDetailCommand;
            }
        }

        #endregion



        #region Property


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

        public double Calories
        {
            get { return Sport.Calories; }
            set
            {
                if (Sport.Calories != value)
                {
                    Sport.Calories = value;
                    RaisePropertyChanged(() => Calories);
                }
            }
        }

        public int Minutes
        {
            get { return Sport.Minutes; }
            set
            {
                if (value != Sport.Minutes)
                {
                    Sport.Minutes = value;
                    RaisePropertyChanged(() => Minutes);
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
                    RaisePropertyChanged(() => Brief);
                }
            }
        }

        #endregion


        #region IDragSource Members

        public void StartDrag(DragInfo dragInfo)
        {
            dragInfo.Data = Sport;
            dragInfo.Effects = DragDropEffects.All;
        }

        #endregion

    }
}
