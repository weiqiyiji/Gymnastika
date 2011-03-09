using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services.Providers
{
    public interface ICategoryProvider : IProvider<SportsCategory>
    {

    }

    public class CategoryProvider : ProviderBase<SportsCategory>, ICategoryProvider
    {
        public CategoryProvider(IRepository<SportsCategory> repository, IWorkEnvironment environment)
            : base(repository, environment)
        {
        }
    }
}
