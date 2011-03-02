using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.Events;

namespace Gymnastika.Modules.Sports.Events
{
    public class CategoryChangedEvent : CompositePresentationEvent<SportsCategory>
    {

    }
}
