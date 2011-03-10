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

        Sport Sport { get; }

        double Calories { get; }

        int Minutes { get; }

        ICommand ShowDetailCommand { get; }
        ICommand AddToFavouriteCommand { get; }
        ICommand AddToPlanCommand { get; }
        
        event EventHandler ShowDetailEvent;
        event EventHandler AddToFavouriteEvent;
        event EventHandler AddToPlanEvent;


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
        }

        public event EventHandler ShowDetailEvent = delegate { };
        public event EventHandler AddToFavouriteEvent = delegate { };
        public event EventHandler AddToPlanEvent = delegate { };


        #region Command
        ICommand _showDetailCommand;
        public ICommand ShowDetailCommand 
        {
            get
            {
                if (_showDetailCommand == null)
                    _showDetailCommand = new DelegateCommand(ShowDetail);
                return _showDetailCommand;
            }
        }

        ICommand _addToFavouriteCommand;
        public ICommand AddToFavouriteCommand 
        {
            get
            {
                if (_addToFavouriteCommand == null)
                    _addToFavouriteCommand = new DelegateCommand(AddToFavourite);
                return _addToFavouriteCommand;
            }
        }

        ICommand _addToPlanCommand;
        public ICommand AddToPlanCommand
        {
            get
            {
                if (_addToPlanCommand==null)
                    _addToPlanCommand = new DelegateCommand(AddToPlan);
                return _addToPlanCommand;
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


        #region Function

        void ShowDetail()
        {
            if (ShowDetailEvent != null)
                ShowDetailEvent(this, EventArgs.Empty);
        }

        void AddToFavourite()
        {
            if (AddToFavouriteEvent != null)
                AddToFavouriteEvent(this, EventArgs.Empty);
        }

        void AddToPlan()
        {
            if (AddToPlanEvent != null)
                AddToPlanEvent(this, EventArgs.Empty);
        }

        #endregion
    }
}
