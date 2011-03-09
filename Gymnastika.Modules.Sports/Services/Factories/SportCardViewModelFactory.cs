using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services.Factories
{
    public interface ISportCardViewModelFactory
    {
        ISportCardViewModel Create(Sport sport);
    }

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
