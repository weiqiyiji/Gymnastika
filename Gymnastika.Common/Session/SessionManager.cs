using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Session
{
    public class SessionManager : ISessionManager
    {
        private readonly IDictionary<int, SessionContext> _sessions = new Dictionary<int, SessionContext>();
        private SessionContext _currentSession;

        #region ISessionManager Members

        public SessionContext GetCurrentSession()
        {
            return _currentSession;
        }

        public void Add(UserModel user)
        {
            if(user == null) throw new ArgumentNullException("user");

            if (!_sessions.ContainsKey(user.Id))
            {
                _currentSession = new SessionContext(user);
                _sessions.Add(user.Id, _currentSession);
            }
            else
            {
                SessionContext context = _sessions[user.Id];
                context.Timestamp = DateTime.Now;

                _currentSession = context;
            }
        }

        public void Remove(UserModel user)
        {
            if (user == null) throw new ArgumentNullException("user");

            if (user.Id == _currentSession.AssociatedUser.Id)
            {
                _currentSession = null;
            }

            _sessions.Remove(user.Id);
        }

        #endregion
    }
}
