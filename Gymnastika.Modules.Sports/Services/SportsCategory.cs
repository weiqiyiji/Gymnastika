using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Services;
using System.ComponentModel.Composition;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    [Export(typeof(ISportsCategory))]
    public class SportsCategory : ISportsCategory
    {

        #region ISportsCategory Members

        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUri { set; get; }

        public IEnumerable<Sport> Sports { get; set; }

        #endregion
    }
}
