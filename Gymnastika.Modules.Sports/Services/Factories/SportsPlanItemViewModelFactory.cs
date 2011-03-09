using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services.Factories
{
    public interface ISportsPlanItemViewModelFactory
    {
        ISportsPlanItemViewModel Create(SportsPlanItem item);
    }

    public class SportsPlanItemViewModelFactory : ISportsPlanItemViewModelFactory
    {
        #region ISportsPlanItemViewModelFactory Members

        public ISportsPlanItemViewModel Create(SportsPlanItem item)
        {
            return new SportsPlanItemViewModel(item);
        }
        #endregion
    }
}
