using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public interface ISportsPlanItemViewModelFactory
    {
        ISportsPlanItemViewModel Create(SportsPlanItem item);
    }
}
