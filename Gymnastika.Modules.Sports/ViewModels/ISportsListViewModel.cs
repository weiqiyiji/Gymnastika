using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Controls;
using Gymnastika.Modules.Sports.Services;

namespace Gymnastika.Modules.Sports.Views
{
    public interface ISportsListViewModel
    {
        ObservableCollection<ISportsCategory> Categories { get; set; }
    }
}
