using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Session
{
    public interface ISessionManager
    {
        SessionContext GetCurrentSession();
        void Add(UserModel user);
        void Remove(UserModel user);
    }
}
