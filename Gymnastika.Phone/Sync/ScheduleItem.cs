using System;
using System.Collections.Generic;
using System.Linq;

namespace Gymnastika.Sync.Communication
{
    public class ScheduleItem
    {
        public int UserId { get; set; }
        public int ConnectionId { get; set; }
        public DateTime StartTime { get; set; }
        public string Message { get; set; }
    }
}