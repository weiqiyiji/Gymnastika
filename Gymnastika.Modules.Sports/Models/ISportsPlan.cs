using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gymnastika.Modules.Sports.Interface
{
    /// <summary>
    /// A sports plan
    /// </summary>
    public interface ISportsPlan
    {
        IList<ISportsPlanItem> SportsPlanItems { get; set; }
        //add extra information here
    }
}
