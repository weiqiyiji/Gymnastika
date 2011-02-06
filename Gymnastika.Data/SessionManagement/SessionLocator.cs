using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Logging;
using NHibernate;

namespace Gymnastika.Data.SessionManagement
{
    public class SessionLocator
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private ISession _session;

        public SessionLocator(
            ISessionFactoryHolder sessionFactoryHolder)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public ISession For(Type entityType)
        {
            Logger.Debug("Acquiring session for {0}", entityType);

            if (_session == null)
            {

                var sessionFactory = _sessionFactoryHolder.GetSessionFactory();

                Logger.Information("Openning database session");
                _session = sessionFactory.OpenSession();
            }
            return _session;
        }
    }
}
