using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.SessionManagement;
using NHibernate;

namespace Gymnastika.Data.Tests.Mocks
{
    public class MockSessionLocator : ISessionLocator
    {
        private ISessionFactoryHolder _sessionFactoryHolder;
        private IDictionary<Type, ISession> _sessions;

        public MockSessionLocator(ISessionFactoryHolder sessionFactoryHolder)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
            _sessions = new Dictionary<Type, ISession>();
        }

        #region ISessionLocator Members

        public ISession For(Type entityType)
        {
            ISession session = null;

            if (!_sessions.TryGetValue(entityType, out session))
            {
                session = _sessionFactoryHolder.GetSessionFactory().OpenSession();
                _sessions.Add(entityType, session);
            }

            return session;
        }

        public void CloseSession(Type entityType)
        {
            ISession session = null;

            if (_sessions.TryGetValue(entityType, out session))
            {
                session.Close();
                _sessions.Remove(entityType);
            }
        }

        #endregion
    }
}
