using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class SportsPlanProvider : ISportsPlanProvider
    {
        IEnumerable<SportsPlan> SportsCategories { get; set; }
    }
}
