using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Models
{
    public class TaskItem
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public int Score { get; set; }

        public bool Mark { get; set; }
    }
}
