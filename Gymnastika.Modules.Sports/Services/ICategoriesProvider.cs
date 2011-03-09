using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public interface ICategoriesProvider : IProvider<SportsCategory>
    {
        //IEnumerable<SportsCategory> Fetch(Func<SportsCategory,bool> predicate);
    }
}
