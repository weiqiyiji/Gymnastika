using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface IPlanDetailViewModel
    {
        void SetPlan(SportsPlan plan);
    }

    public class PlanDetailViewModel : IPlanDetailViewModel
    {
        public void SetPlan(SportsPlan plan)
        {
            throw new NotImplementedException();
        }
    }
}
