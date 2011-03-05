using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.ComponentModel;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsPlanItemViewModel : INotifyPropertyChanged
    {
        event EventHandler CloseViewRequest;
        SportsPlanItem Item { get; set; }
    }
}
