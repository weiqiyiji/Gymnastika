using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Events
{
    class SportSelectDateEvent : CompositePresentationEvent<DateTime>
    {
    }
}
