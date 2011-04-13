using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlanItem
    {
        public SportsPlanItem()
        {
            DateTime now = DateTime.Now;
            Hour = now.Hour;
            Minute = now.Minute;
        }

        public virtual int Id { get; set; }

        public virtual int Hour { get; set; }

        public virtual int Minute { get; set; }

        public virtual int Duration { get; set; }   //Min
        
        public virtual bool Completed { get; set; }

        //public virtual int SportId { get; set; }  

        public virtual Sport Sport { get; set; }

        public virtual double Score { get; set;}

        public virtual SportsPlan SportsPlan { get; set; }
    }
}
