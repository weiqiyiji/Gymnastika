using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services.Providers
{
    public interface ICategoriesProvider : IProvider<SportsCategory>
    {

    }

    public class CategoriesProvider : DbProviderBase<SportsCategory>, ICategoriesProvider
    {

    }    
}
