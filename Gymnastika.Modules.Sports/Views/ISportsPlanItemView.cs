using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports.Views
{
    public interface ISportsPlanItemView
    {
        ISportsPlanItemViewModel ViewModel { get; set; }
    }
}
