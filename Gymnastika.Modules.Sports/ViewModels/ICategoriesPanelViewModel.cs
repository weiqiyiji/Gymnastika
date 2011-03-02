using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ICategoriesPanelViewModel
    {
        ObservableCollection<SportsCategory> Categories { get; set; }

        SportsCategory CurrentSelectedItem { get; set; }
    }
}
