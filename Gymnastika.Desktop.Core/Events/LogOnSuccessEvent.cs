using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Desktop.Core.UserManagement;
using Microsoft.Practices.Prism.Events;

namespace Gymnastika.Desktop.Core.Events
{
    public class LogOnSuccessEvent : CompositePresentationEvent<User>
    {
    }
}
