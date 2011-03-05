using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports.Services
{
    public class SportCardViewModelFactory : ISportCardViewModelFactory
    {
        #region ISportCardViewModelFactory Members

        public ViewModels.ISportCardViewModel Create(Models.Sport sport)
        {
            return new SportCardViewModel(sport);
        }

        #endregion
    }
}
