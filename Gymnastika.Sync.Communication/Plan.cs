using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Sync.Communication
{
    public class Plan
    {
        public int UserId { get; set; }
        public ScheduleItemCollection ScheduleItems { get; set; }
    }
}
