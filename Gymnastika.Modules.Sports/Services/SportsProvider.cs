using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Gymnastika.Modules.Sports.Services
{
    [Export(typeof(ISportsProvider))]
    public class SportsProvider : ISportsProvider
    {

        #region ISportsProvider Members

        public IEnumerable<ISportsCategory> SportsCategories
        {
            get
            {
                return new List<ISportsCategory>();
            }
        }

        #endregion
    }
}
