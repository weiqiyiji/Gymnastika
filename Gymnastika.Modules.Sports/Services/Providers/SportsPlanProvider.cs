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
        IEnumerable<SportsPlan> Fetch(DateTime date);
        SportsPlan FetchFirstOrDefault(DateTime date);
    }

    public class SportsPlanProvider : ProviderBase<SportsPlan>, ISportsPlanProvider
    {
        public SportsPlanProvider(IRepository<SportsPlan> repository, IWorkEnvironment environment)
            : base(repository, environment)
        {
        }


        #region ISportsPlanProvider Members

        public IEnumerable<SportsPlan> Fetch(DateTime date)
        {
            return Fetch(t => t.Year == date.Year && t.Month == date.Month && t.Day == date.Day);
        }

        public SportsPlan FetchFirstOrDefault(DateTime date)
        {
            return Fetch(date).FirstOrDefault();
        }

        #endregion
    }
}
