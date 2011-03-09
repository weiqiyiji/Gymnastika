using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using Gymnastika.Services.Models;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlan
    {
        public SportsPlan()
        {
            Time = DateTime.Now;
            SportsPlanItems = new List<SportsPlanItem>();
        }

        public virtual int Id { set; get; }

        public virtual DateTime Time { get; set; }

        public virtual int Score { get; set; }

        public virtual IList<SportsPlanItem> SportsPlanItems { get; set; }

        public virtual User User { get; set; }
    }
}
