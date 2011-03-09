using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services.Providers
{
    public interface ISportProvider : IProvider<Sport>
    {
    }

    public class SportProvider : DbProviderBase<Sport>, IProvider<Sport>
    {
    }
}
