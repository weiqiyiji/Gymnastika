using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlanItem
    {
        public Sport Sport { get; set; }

        public int SportsTime_Hour { set; get; }

        public int SportsTime_Min { get; set; }

        public int Duration { get; set; }   //Min

        public bool Completed { get; set; }

    }
}
