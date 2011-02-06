using NHibernate.Tool.hbm2ddl;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data.Providers;
using Gymnastika.Common.Configuration;

namespace Gymnastika.Data.Migration.Generator
{
    public class SchemaCommandGenerator : ISchemaCommandGenerator
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private readonly ShellSettings _shellSettings;
        private readonly IDataServicesProviderFactory _dataServicesProviderFactory;

        public SchemaCommandGenerator(
            ISessionFactoryHolder sessionFactoryHolder,
            ShellSettings shellSettings,
            IDataServicesProviderFactory dataServicesProviderFactory)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
            _shellSettings = shellSettings;
            _dataServicesProviderFactory = dataServicesProviderFactory;
        }

        /// <summary>
        /// Automatically updates a db to a functionning schema
        /// </summary>
        public void UpdateDatabase()
        {
            var configuration = _sessionFactoryHolder.GetConfiguration();
            new SchemaUpdate(configuration).Execute(false, true);
        }

        /// <summary>
        /// Automatically creates a db with a functionning schema
        /// </summary>
        public void CreateDatabase()
        {
            var configuration = _sessionFactoryHolder.GetConfiguration();
            new SchemaExport(configuration).Execute(false, true, false);
        }

    }
}
