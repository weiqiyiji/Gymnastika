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
using FluentNHibernate.Conventions.Helpers;
using Gymnastika.Common.Utils;
using Gymnastika.Data.Conventions;

namespace Gymnastika.Data.Providers
{
    public class SqlCeDataServicesProvider : AbstractDataServicesProvider
    {
        private readonly string _fileName;
        private readonly string _dataFolder;

        public IAutomappingConfigurer AutomappingConfigurer { get; set; }

        public SqlCeDataServicesProvider(string dataFolder, string dbName)
        {
            if (string.IsNullOrEmpty(dataFolder))
                throw new DataServicesProviderInitializationException(this, "dataFolder missing");

            if (string.IsNullOrEmpty(dbName))
                throw new DataServicesProviderInitializationException(this, "dbName missing");

            _dataFolder = dataFolder;
            _fileName = Path.Combine(
                _dataFolder, 
                dbName.EndsWith("sdf") ? dbName : (dbName + ".sdf"));
        }

        public SqlCeDataServicesProvider(string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
                throw new DataServicesProviderInitializationException(this, "fileName missing");

            _dataFolder = Path.GetDirectoryName(fileName);
            _fileName = fileName;
        }

        public static string ProviderName
        {
            get { return "SqlCe"; }
        }

        protected override FluentConfiguration InnerConfiguration(FluentConfiguration cfg)
        {
            if (this.AutomappingConfigurer == null) return cfg;

            var autoMappingMetadataCollection = AutomappingConfigurer.GetAutomappingMetadata();
            var persistenceAssemblies = autoMappingMetadataCollection
                .Select(metadata => Assembly.LoadFrom(metadata.AssemblyName));

            return cfg.Mappings(
                m => m.AutoMappings.Add(
                    AutoMap.Assemblies(new AutomappingConfigurationFilter(), persistenceAssemblies)
                        .Conventions
                        .Setup(c =>
                        {
                            c.Add(PrimaryKey.Name.Is(x => "Id")); 
                            c.Add(ForeignKey.Format((x, t) => t.Name + "Id"));
                            c.Add(DefaultLazy.Always());
                            c.AddFromAssemblyOf<TablePluralizationConvention>();
                        }))
                    .ExportTo(Directory.GetCurrentDirectory()));
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

            return persistence.ConnectionString(localConnectionString);
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
