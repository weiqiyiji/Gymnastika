using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services.Providers
{
    public interface ISportProvider : IProvider<Sport>
    {
        IEnumerable<Sport> Fetch(SportsCategory category, Func<Sport, bool> predicate = null);
        IEnumerable<Sport> Fetch(SportsCategory category, int startIndex, int number, Func<Sport, bool> predicate = null);
    }

    public class SportProvider : ProviderBase<Sport>, ISportProvider
    {
        public SportProvider(IRepository<Sport> repository, IWorkEnvironment environment)
            : base(repository, environment)
        {
        }


        public IEnumerable<Sport> Fetch(SportsCategory category, Func<Sport, bool> predicate = null)
        {

            return Fetch(s => (s.SportsCategories.Contains(category) && predicate(s)));
        }

        public IEnumerable<Sport> Fetch(SportsCategory category, int startIndex, int number, Func<Sport, bool> predicate = null)
        {
            return Fetch(category, predicate).Skip(startIndex).Take(number);
        }

    }
}
