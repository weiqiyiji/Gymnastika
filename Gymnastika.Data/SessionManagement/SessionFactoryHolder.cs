using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using Gymnastika.Common.Logging;
using Gymnastika.Data.Providers;
using Gymnastika.Common.Configuration;

namespace Gymnastika.Data.SessionManagement
{
    public class SessionFactoryHolder : ISessionFactoryHolder
    {
        private readonly ShellSettings _shellSettings;
        private readonly IDataServicesProviderFactory _dataServicesProviderFactory;

        private ISessionFactory _sessionFactory;
        private NHibernate.Cfg.Configuration _configuration;

        public SessionFactoryHolder(
            ShellSettings shellSettings,
            IDataServicesProviderFactory dataServicesProviderFactory)
        {
            _shellSettings = shellSettings;
            _dataServicesProviderFactory = dataServicesProviderFactory;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public ISessionFactory GetSessionFactory()
        {
            lock (this)
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = BuildSessionFactory();
                }
            }
            return _sessionFactory;
        }

        public NHibernate.Cfg.Configuration GetConfiguration()
        {
            lock (this)
            {
                if (_configuration == null)
                {
                    _configuration = BuildConfiguration();
                }
            }
            return _configuration;
        }

        private ISessionFactory BuildSessionFactory()
        {
            Logger.Debug("Building session factory");

            NHibernate.Cfg.Configuration config = GetConfiguration();
            return config.BuildSessionFactory();
        }

        private NHibernate.Cfg.Configuration BuildConfiguration()
        {
            var parameters = GetDataServicesParameters();

            return _dataServicesProviderFactory
                    .CreateProvider(parameters)
                    .BuildConfiguration(parameters);
        }

        private DataServiceParameters GetDataServicesParameters()
        {
            return new DataServiceParameters
            {
                Provider = _shellSettings.DataProvider,
                ConnectionString = _shellSettings.DataConnectionString
            };
        }
    }
}
