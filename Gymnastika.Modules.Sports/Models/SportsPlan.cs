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
            DateTime now = DateTime.Now;
            Year = now.Year;
            Month = now.Month;
            Day = now.Day;
            SportsPlanItems = new List<SportsPlanItem>();
            SynchronizedToServer = false;
        }

        public virtual int Id { set; get; }

        public virtual int Year{ get; set; }

        public virtual int Month { get; set; }

        public virtual int Day { get; set; }

        public virtual int Score { get; set; }

        public virtual IList<SportsPlanItem> SportsPlanItems { get; set; }

        public virtual bool SynchronizedToServer { get; set; }

        public virtual User User { get; set; }
    }
}
