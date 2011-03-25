using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using Gymnastika.Sync.Models;

namespace Gymnastika.Sync.Schedule
{
    public class TimerMetadata
    {
        public Timer Timer { get; set; }
        public string TargetUri { get; set; }
        public ScheduleItem Schedule { get; set; }
    }
}