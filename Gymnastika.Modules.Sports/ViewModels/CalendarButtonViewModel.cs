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
    }

    public class CalendarButtonViewModel : ICalendarButtonViewModel
    {
        public CalendarButtonViewModel(SportsPlan plan)
        {
            Plan = plan;
        }

        public SportsPlan Plan { get; set; }
    }
}
