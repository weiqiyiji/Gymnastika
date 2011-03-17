using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Data.SessionManagement;
using NHibernate;
using Gymnastika.Common.Logging;

namespace Gymnastika.Sync.Infrastructure
{
    public class WcfSessionLocator : ISessionLocator
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private readonly ILogger _logger;

        public WcfSessionLocator(ISessionFactoryHolder sessionFactoryHolder, ILogger logger)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
            _logger = logger;
        }

        #region ISessionLocator Members

        public ISession For(Type entityType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}