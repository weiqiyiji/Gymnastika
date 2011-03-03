using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsPlanItemViewModel
    {
        event EventHandler CloseViewRequest;
        SportsPlanItem Item { get; set; }
    }
}
