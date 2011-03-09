using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services.Providers
{

    public interface ISportsPlanProvider : IProvider<SportsPlan>
    {

    }

    public class SportsPlanProvider : ProviderBase<SportsPlan>, ISportsPlanProvider
    {
        public SportsPlanProvider(IRepository<SportsPlan> repository, IWorkEnvironment environment)
            : base(repository, environment)
        {
        }
    }
}
