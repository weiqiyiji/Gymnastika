using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Sync.Communication
{
    public class Task
    {
        public DateTime StartTime { get; set; }
        public int TaskId { get; set; }
    }

    public class TaskList : List<Task>
    {
    }
}
