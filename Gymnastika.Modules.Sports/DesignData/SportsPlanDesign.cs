using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.DesignData
{
    public class SportsPlanDesign
    {
        public SportsPlanDesign()
        {
            SportsPlanItems = new ObservableCollection<SportsPlanItem>();
        }
        public ObservableCollection<SportsPlanItem> SportsPlanItems
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }
    }
}
