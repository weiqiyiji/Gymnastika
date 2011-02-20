using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    [Export(typeof(ISportsProvider))]
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
