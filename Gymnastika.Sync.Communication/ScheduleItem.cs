using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Communication
{
    public class ScheduleItem
    {
        public int ConnectionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public object Message { get; set; }
    }
}