using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Services.Models;

namespace Gymnastika.Services.Session
{
    public interface ISessionManager
    {
        SessionContext GetCurrentSession();
        event EventHandler SessionChanged;
        void Add(User user);
        void Remove(User user);
        IEnumerable<SessionContext> GetAllActiveSessions();
    }
}
