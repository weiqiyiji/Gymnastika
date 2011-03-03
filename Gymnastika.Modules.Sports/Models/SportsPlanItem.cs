using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlanItem
    {
        public virtual int Id { get; set; }

        public virtual DateTime Time { get; set; }

        public virtual int Duration { get; set; }   //Min
        
        public virtual bool Completed { get; set; }

        public virtual int SportId { get; set; }  

        public virtual Sport Sport { get; set; }

        public virtual int SportsPlanId { get; set; }
    }
}
