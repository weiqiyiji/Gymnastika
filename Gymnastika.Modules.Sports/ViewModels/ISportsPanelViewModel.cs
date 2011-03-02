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
    
        void CategoryChanged(SportsCategory category);
        
        ObservableCollection<Sport> Sports { get; set; }
    
    }
}
