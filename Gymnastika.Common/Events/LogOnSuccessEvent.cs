using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.UserManagement;
using Microsoft.Practices.Prism.Events;

namespace Gymnastika.Common.Events
{
    public class LogOnSuccessEvent : CompositePresentationEvent<User>
    {
    }
}
