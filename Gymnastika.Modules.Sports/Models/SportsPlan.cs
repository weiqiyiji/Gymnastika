using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlan
    {
        IList<SportsPlanItem> SportsPlanItems { get; set; }
    }
}
