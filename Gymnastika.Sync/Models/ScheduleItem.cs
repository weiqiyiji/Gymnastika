using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class ScheduleItem
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ConnectionId { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string Message { get; set; }
        public virtual bool IsComplete { get; set; }
    }
}