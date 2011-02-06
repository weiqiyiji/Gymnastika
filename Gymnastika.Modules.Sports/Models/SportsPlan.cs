using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlan
    {
        public string Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int Score { get; set; }

        public IList<SportsPlanItem> SportsPlanItems { get; set; }
    }
}
