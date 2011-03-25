using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Services.Providers;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Events;
using Gymnastika.Modules.Sports.Facilities;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface IPlanDetailViewModel
    {
        SportsPlan SportsPlan { get; }
        void SetPlan(SportsPlan plan);
        event EventHandler PlanChangedEvent;
        DelegateCommand NextDayCommand { get; }
        DelegateCommand LastDayCommand { get; }
        string Date { get; }
    }

    public class PlanDetailViewModel : NotificationObject , IPlanDetailViewModel
    {
        readonly ISportsPlanProvider _sportsPlanProvider;
        readonly IPlanItemProvider _itemProvider;
        readonly ISportProvider _sportProvider;
        readonly IEventAggregator _eventAggregator;

        public PlanDetailViewModel(ISportsPlanProvider planProvider,IPlanItemProvider itemProvider,ISportProvider sportProvider,IEventAggregator eventAggregator)
        {
            _sportsPlanProvider = planProvider;
            _itemProvider = itemProvider;
            _sportProvider = sportProvider;
            GotoDay(DateTime.Now);
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SportsPlanCreatedOrModifiedEvent>().Subscribe(OnPlanChanged);
        }

        public string Date
        {
            get 
            {
                DateTime date = new DateTime(SportsPlan.Year, SportsPlan.Month, SportsPlan.Day);
                return DateFacility.GetShortDate(date) + " " + DateFacility.GetDayName(date.DayOfWeek); 
            }
        }

        DelegateCommand _nextDayCommand;
        public DelegateCommand NextDayCommand 
        {
            get
            {
                if (_nextDayCommand == null)
                    _nextDayCommand = new DelegateCommand(GotoNextDay);
                return _nextDayCommand;
            }
        }

        void GotoNextDay()
        {
            GotoDay(CurrentDate.AddDays(1));
        }

        DateTime CurrentDate
        {
            get { return new DateTime(SportsPlan.Year, SportsPlan.Month, SportsPlan.Day); }
        }

        void GotoDay(DateTime date)
        {
            var plan = GetPlanOfOneDay(date);
            UpdatePlan(plan);
        }

        SportsPlan GetPlanOfOneDay(DateTime date)
        {
            SportsPlan planOfOneday;
            using (_sportProvider.GetContextScope())
            {
                planOfOneday = _sportsPlanProvider.FetchFirstOrDefault(date);
                if (planOfOneday != null)
                {
                    planOfOneday.SportsPlanItems = planOfOneday.SportsPlanItems.ToList();
                    foreach (var item in planOfOneday.SportsPlanItems)
                    {
                        item.Sport = _sportProvider.Get(item.Sport.Id);
                    }
                }
            }
            return planOfOneday ?? new SportsPlan() { Year = date.Year, Month = date.Month, Day = date.Day };
        }

        DelegateCommand _lastDayCommand;
        public DelegateCommand LastDayCommand
        {
            get
            {
                if (_lastDayCommand == null)
                    _lastDayCommand = new DelegateCommand(GotoLastDay);
                return _lastDayCommand;
            }
        }

        void GotoLastDay()
        {
            GotoDay(CurrentDate.AddDays(-1));
        }

        void OnPlanChanged(SportsPlan plan)
        {
            if (MathFacility.TheSameDay(plan,SportsPlan))
            {
                UpdatePlan(plan);
            }
        }

        void UpdatePlan(SportsPlan plan)
        {
            _sportsPlan = plan;
            PlanChangedEvent(this, EventArgs.Empty);
            RaisePropertyChanged(() => SportsPlan);
            RaisePropertyChanged(() => Date);
        }

        public event EventHandler PlanChangedEvent = delegate { };

        public void SetPlan(SportsPlan plan)
        {
            SportsPlan = plan;
        }

        SportsPlan _sportsPlan = new SportsPlan();
        public  SportsPlan SportsPlan 
        {
            get { return _sportsPlan; }
            private set
            {
                if (_sportsPlan != value)
                {
                    UpdatePlan(value);
                }
            }
        }

    }
}
