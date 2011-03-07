using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportCardViewModel
    {
        String Name { get; }
        double Calories { get; }
        int Minutes { get; }
    }
}
