using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Interface
{
    /// <summary>
    /// A sport plan item
    /// </summary>
    public interface ISportsPlanItem
    {
        ISport Sport { get; set; }

        int Hour { get; set; }          //Plan's time
        int Min { get; set; }
        int Duration { get; set; }      //unit: Min

        bool HasCompleted { get; set; }
        int Score { get; set; }
    }
}
