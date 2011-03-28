using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services.Providers
{

    public interface ISportsPlanProvider : IProvider<SportsPlan>
    {
        IEnumerable<SportsPlan> FetchUnSychronizedItems();
        IEnumerable<SportsPlan> Fetch(DateTime date);
        SportsPlan FetchFirstOrDefault(DateTime date);
        SportsPlan GetWithOutDelay(int id);
    }

    public class SportsPlanProvider : ProviderBase<SportsPlan>, ISportsPlanProvider
    {
        ISportProvider _sportProvider;

        public SportsPlanProvider(IRepository<SportsPlan> repository,ISportProvider sportProvider ,IWorkEnvironment environment)
            : base(repository, environment)
        {
            _sportProvider = sportProvider;
        }

        public SportsPlan GetWithOutDelay(int id)
        {
            SportsPlan plan = Get(id);
            plan.SportsPlanItems = plan.SportsPlanItems.ToList();
            foreach (var item in plan.SportsPlanItems)
            {
                item.Sport = _sportProvider.Get(item.Sport.Id);
            }
            return plan;
        }

        public IEnumerable<SportsPlan> FetchUnSychronizedItems()
        {
            return Fetch(t => t.SynchronizedToServer == false);
        }
        
        public IEnumerable<SportsPlan> Fetch(DateTime date)
        {
            return Fetch(t => t.Year == date.Year && t.Month == date.Month && t.Day == date.Day);
        }

        public SportsPlan FetchFirstOrDefault(DateTime date)
        {
            return Fetch(date).FirstOrDefault();
        }

    }
}
