using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services.Providers
{
    public interface IPlanItemProvider : IProvider<SportsPlanItem>
    {

    }

    public class PlanItemProvider : ProviderBase<SportsPlanItem> , IPlanItemProvider
    {
        public PlanItemProvider(IRepository<SportsPlanItem> repository, IWorkEnvironment environment)
            : base(repository, environment)
        {
        }
    }
}
