using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsPanelViewModel
    {
    
        IList<Sport> Sports { get; }

        Predicate<ISportCardViewModel> Filter { get; set; }

        SportsCategory Category { get; set; }
    }
}
