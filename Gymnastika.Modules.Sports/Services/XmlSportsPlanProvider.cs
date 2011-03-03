using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class XmlSportsPlanProvider : ISportsPlanProvider
    {
        #region ISportsPlanProvider Members

        public IEnumerable<Models.SportsPlan> Fetch(Func<Models.SportsPlan, bool> predicate)
        {
            yield return new SportsPlan()
            {

            };
        }

        #endregion
    }
}
