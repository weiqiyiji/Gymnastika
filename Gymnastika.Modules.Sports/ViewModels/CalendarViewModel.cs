using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.Services.Providers;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ICalendarViewModel
    {
        IList<SportsPlan> Plans { get; }
    }


    public class CalendarViewModel : ICalendarViewModel
    {
        readonly ISportsPlanProvider _sportsPlanProvider;
        readonly ISportProvider _sportProvider;

        public CalendarViewModel(ISportsPlanProvider sportsPlanProvider,ISportProvider sportProvider)
        {
            _sportProvider = sportProvider;
            _sportsPlanProvider = sportsPlanProvider;
            Plans = LoadPlans();
        }

        IList<SportsPlan> LoadPlans()
        {
            IList<SportsPlan> plans = null;
            using (_sportsPlanProvider.GetContextScope())
            {
                plans = _sportsPlanProvider.All().ToList();
                foreach (var plan in plans)
                {
                    foreach (var item in plan.SportsPlanItems)
                        item.Sport = _sportProvider.Get(item.Sport.Id);
                }
            }
            return plans;
        }

        public IList<SportsPlan> Plans { get; set; }
    }
}
