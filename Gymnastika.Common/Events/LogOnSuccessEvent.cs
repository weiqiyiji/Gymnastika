using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Services;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Events
{
    public class LogOnSuccessEvent : CompositePresentationEvent<UserModel>
    {
    }
}
