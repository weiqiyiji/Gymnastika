using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Sports.Services.Providers;

namespace Gymnastika.Modules.Sports.ViewModels
{


    public interface ICalendarButtonViewModel
    {
        SportsPlan Plan { get; }

        IList<SportsPlanItem> Items { get; }

        DateTime Date { get; }
    }

    public class CalendarButtonViewModel : ICalendarButtonViewModel
    {
        public CalendarButtonViewModel(SportsPlan plan,DateTime date)
        {
            Plan = plan;
            Date = date;
        }

        public DateTime Date { get; set; }

        public SportsPlan Plan { get; set; }

        public IList<SportsPlanItem> Items
        {
            get 
            {
                if (Plan == null)
                    return null;
                else
                    return Plan.SportsPlanItems; 
            }
        }
    }
}
