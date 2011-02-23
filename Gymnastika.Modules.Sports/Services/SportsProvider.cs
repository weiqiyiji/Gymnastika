using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class SportsProvider : ISportsProvider
    {
        #region ISportsProvider Members

        public IEnumerable<SportsCategory> SportsCategories
        {
            get
            {
                return new List<SportsCategory>();
            }
        }

        #endregion
    }
}
