using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Services.Models;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Services.Session
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

        public void Add(User user)
        {     
            if(user == null) throw new ArgumentNullException("user");

            if (!_sessions.ContainsKey(user.Id))
            {
                _currentSession = new SessionContext(user);
                _sessions.Add(user.Id, _currentSession);
                _currentSession.Timestamp = DateTime.Now;
                
                var userRepository = ServiceLocator.Current.GetInstance<IRepository<User>>();
                User savedUser = userRepository.Get(u => u.Id == user.Id);
                savedUser.IsActive = true;

                OnSessionChanged();
            }
            else
            {
                SessionContext context = _sessions[user.Id];
                context.Timestamp = DateTime.Now;

                if (_currentSession != context)
                {
                    _currentSession = context;
                    OnSessionChanged();
                }
            }
        }

        public void Remove(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            if (user.Id == _currentSession.AssociatedUser.Id)
            {
                _currentSession = null;
            }

            _sessions.Remove(user.Id);

            using (ServiceLocator.Current.GetInstance<IWorkEnvironment>().GetWorkContextScope())
            {
                var userRepository = ServiceLocator.Current.GetInstance<IRepository<User>>();
                User savedUser = userRepository.Get(u => u.Id == user.Id);
                savedUser.IsActive = false;
            }
        }

        public IEnumerable<SessionContext> GetAllActiveSessions()
        {
            return _sessions.Values;
        }

        public event EventHandler SessionChanged;

        #endregion

        private void OnSessionChanged()
        {
            if (SessionChanged != null)
                SessionChanged(this, EventArgs.Empty);
        }
    }
}
