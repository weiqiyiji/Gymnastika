using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using GongSolutions.Wpf.DragDrop;
using Gymnastika.Modules.Sports.Models;
using System.Windows;
using Gymnastika.Modules.Sports.Services.Factories;
using Microsoft.Practices.Prism.Commands;
using System.Text.RegularExpressions;

namespace Gymnastika.Modules.Sports.ViewModels
{

    public class AddToPlanEventArgs : EventArgs
    {
        public SportsPlanItem Item;
    }

    public class ShowSportsDetailEventArgs: EventArgs
    {
        public Sport Sport;
    }

    public interface ISportCalorieChartViewModel
    {
        int CaloriePerHour { get; }

        int CompareCaloriePerHour { get; }

        Sport Sport { get; set; }

        Sport CompareSport { get; set; }

        int Duration { get; set;}

        DelegateCommand AddToPlanCommand { get; }

        DelegateCommand ShowDetailCommand { get; }

        event EventHandler<AddToPlanEventArgs> RequestAddToPlanEvent;

        event EventHandler<ShowSportsDetailEventArgs> RequestShowDetailEvent;

        int Hour { get;set; }

        int Minute { get;set; }
    }

    public class SportCalorieChartViewModel : NotificationObject , ISportCalorieChartViewModel , IDropTarget , IDragSource
    {
        public SportCalorieChartViewModel()
        {
            Duration = 30;
            //Sport = new Sport();
            DateTime date = DateTime.Now;
            Hour = date.Hour;
            Minute = date.Minute;
        }



        DelegateCommand _addToPlanCommand;
        public DelegateCommand AddToPlanCommand
        {
            get
            {
                if (_addToPlanCommand == null)
                    _addToPlanCommand = new DelegateCommand(AddToPlan,CanAddToPlan);
                return _addToPlanCommand;

            } 
        }

        public event EventHandler<AddToPlanEventArgs> RequestAddToPlanEvent = delegate { };

        bool CanAddToPlan()
        {
            return Sport != null && Duration > 0;
        }

        void AddToPlan()
        {
            if (RequestAddToPlanEvent != null)
                RequestAddToPlanEvent(this, new AddToPlanEventArgs() { Item = GetPlanItem() });
        }

        DelegateCommand _showDetailCommand;
        public DelegateCommand ShowDetailCommand 
        {
            get
            {
                if (_showDetailCommand == null)
                    _showDetailCommand = new DelegateCommand(ShowDetail,CanShowDetail);
                return
                    _showDetailCommand;
            }
        }

        public event EventHandler<ShowSportsDetailEventArgs> RequestShowDetailEvent = delegate { };

        bool CanShowDetail()
        {
            return Sport != null;
        }
        void ShowDetail()
        {
            if (RequestShowDetailEvent != null)
                RequestShowDetailEvent(this, new ShowSportsDetailEventArgs() { Sport = this.Sport });
        }

        public void DragOver(DropInfo dropInfo)
        {
            var sport = TryGetSport(dropInfo.Data);
            if (sport != null)
            {
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                CompareSport = sport;
            }
        }

        public void Drop(DropInfo dropInfo)
        {
            var sport = TryGetSport(dropInfo.Data);
            if (sport != null)
                Sport = sport;
        }

        Sport TryGetSport(object data)
        {
            Sport sport = null;

            if (data is ISportCardViewModel)
                sport = (data as ISportCardViewModel).Sport;
            else if (data is Sport)
                sport = data as Sport;
            else if (data is ISportsPlanItemViewModel)
                sport = (data as ISportsPlanItemViewModel).Item.Sport;
            return sport;
        }

        int GetCaloriePerHour(Sport sport)
        {
                 int value = 0;
                try{value  = (int)(sport.Calories / sport.Minutes * 60);}
                catch(Exception){}
                return value;
        }

        public int CaloriePerHour
        {
            get {return GetCaloriePerHour(Sport);}
        }

        public int CompareCaloriePerHour
        {
            get {return GetCaloriePerHour(CompareSport); }
        }

        Sport _sport;
        public Sport Sport
        {
            get{return _sport;}
            set
            {
                if(_sport != value)
                {
                    _sport = value;
                    RaisePropertyChanged("");
                    RefreshCommandState();
                }
            }
        }

        void RefreshCommandState()
        {
            AddToPlanCommand.RaiseCanExecuteChanged();
            ShowDetailCommand.RaiseCanExecuteChanged();
        }

        Sport _compareSport;
        public Sport CompareSport
        {
            get{ return _compareSport;}
            set
            {
                if(_compareSport!= value)
                {
                    _compareSport = value;
                    RaisePropertyChanged("");
                }
            }
        }

        public int Duration { get; set; }


        bool IsTimeValid(int hour, int minute)
        {
            return (hour >= 0 && minute >= 0 && minute < 60 && hour * 60 + minute <= 24 * 60);
        }


        int _hour;
        public int Hour
        {
            get { return _hour; }
            set
            {
                if (_hour != value)
                {
                    _hour = value;
                    RaisePropertyChanged(() => Hour);
                    RefreshCommandState();
                }
            }
        }

        int _minute;
        public int Minute
        {
            get { return _minute; }
            set
            {
                if (_minute != value)
                {
                    _minute = value;
                    RaisePropertyChanged(() => Minute);
                    RefreshCommandState();
                }
            }
        }

        public void StartDrag(DragInfo dragInfo)
        {
            if (this.Sport != null)
            {
                dragInfo.Data = GetPlanItem();
                dragInfo.Effects = DragDropEffects.All;
            }

        }

        SportsPlanItem GetPlanItem()
        {
            return new SportsPlanItem() { Sport = this.Sport, Hour = this.Hour, Minute = this.Minute, Duration = this.Duration };
        }


    }
}
