using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Services.Models;

namespace Gymnastika.Events
{
    public class LogOnSuccessEvent : CompositePresentationEvent<User>
    {
    }
}
