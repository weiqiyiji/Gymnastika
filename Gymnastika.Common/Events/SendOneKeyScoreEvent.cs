using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Events
{
    public class SendOneKeyScoreEvent : CompositePresentationEvent<List<TaskItem>>
    {
    }
}
