using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Session
{
    public class SessionContext
    {
        public SessionContext(UserModel userModel)
        {
            AssociatedUser = userModel;
            Timestamp = DateTime.Now;
        }

        public UserModel AssociatedUser { get; private set; }

        public DateTime Timestamp { get; set; }
    }
}
