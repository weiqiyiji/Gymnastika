using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Services.Models;

namespace Gymnastika.Services.Session
{
    public class SessionContext
    {
        public SessionContext(User User)
        {
            AssociatedUser = User;
            Timestamp = DateTime.Now;
        }

        public User AssociatedUser { get; private set; }

        public DateTime Timestamp { get; set; }
    }
}
