using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;
using Gymnastika.Data.Configuration;

namespace Gymnastika.Data.Providers
{
    public class SqlCeDataServicesProvider : AbstractDataServicesProvider
    {
        private readonly string _fileName;
        private readonly string _dataFolder;
        private readonly string _connectionString;

        public IAutomappingConfigurer AutomappingConfigurer { get; set; }

        public SqlCeDataServicesProvider(string dataFolder, string connectionString)
        {
            _dataFolder = dataFolder;
            _connectionString = connectionString;
            _fileName = Path.Combine(_dataFolder, "Gymnastika.sdf");
        }

        public SqlCeDataServicesProvider(string fileName)
        {
            _dataFolder = Path.GetDirectoryName(fileName);
            _fileName = fileName;
        }

        public static string ProviderName
        {
            get { return "SqlCe"; }
        }

        protected override FluentConfiguration Configuration(FluentConfiguration cfg)
        {
            if (this.AutomappingConfigurer == null) return cfg;

            var autoMappingMetadataCollection = AutomappingConfigurer.GetAutomappingAssemblies();
            var persistenceModels = autoMappingMetadataCollection
                .Select(metadata => Assembly.LoadFrom(metadata.AssemblyName));
            
            return cfg.Mappings(
                        m => m.AutoMappings.Add(
                            AutoMap.Assemblies(new AutomappingConfigurationFilter(), persistenceModels)));
        }

        public override IPersistenceConfigurer GetPersistenceConfigurer(bool createDatabase)
        {
            var persistence = MsSqlCeConfiguration.Standard;

            if (createDatabase)
            {
                File.Delete(_fileName);
            }

            string localConnectionString = string.Format("Data Source={0}", _fileName);
            if (!File.Exists(_fileName))
            {
                CreateSqlCeDatabaseFile(localConnectionString);
            }

            return persistence.ConnectionString(localConnectionString)
                              .Driver(typeof(SqlServerCeDriver).AssemblyQualifiedName);
        }

        private void CreateSqlCeDatabaseFile(string connectionString)
        {
            if (!string.IsNullOrEmpty(_dataFolder))
                Directory.CreateDirectory(_dataFolder);

            // We want to execute this code using Reflection, to avoid having a binary
            // dependency on SqlCe assembly
            const string assemblyName = "System.Data.SqlServerCe";
            const string typeName = "System.Data.SqlServerCe.SqlCeEngine";

            var sqlceEngineHandle = Activator.CreateInstance(assemblyName, typeName);
            var engine = sqlceEngineHandle.Unwrap();

            //engine.LocalConnectionString = connectionString;
            engine.GetType().GetProperty("LocalConnectionString").SetValue(engine, connectionString, null/*index*/);

            //engine.CreateDatabase();
            engine.GetType().GetMethod("CreateDatabase").Invoke(engine, null);

            //engine.Dispose();
            engine.GetType().GetMethod("Dispose").Invoke(engine, null);
        }
    }
}
