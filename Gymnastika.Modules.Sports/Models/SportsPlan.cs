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
        public ObservableCollection<SportsPlanItem> SportsPlanItems { get; set; }
    }
}
