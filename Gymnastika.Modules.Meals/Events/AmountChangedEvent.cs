using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace Gymnastika.Modules.Meals.Events
{
    public class AmountChangedEvent : CompositePresentationEvent<int>
    {
    }
}
