using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.DesignData
{
    public class PlansDesign
    {
        public PlansDesign()
        {
            Plans = new ObservableCollection<SportsPlan>();
        }
        public ObservableCollection<SportsPlan> Plans { get; set; }
    }
}
