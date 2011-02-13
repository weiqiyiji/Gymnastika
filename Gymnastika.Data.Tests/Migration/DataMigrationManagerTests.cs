using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gymnastika.Common.Configuration;
using Gymnastika.Common.Logging;
using Gymnastika.Data.Configuration;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Models;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data.Tests.Mocks;
using Gymnastika.Data.Tests.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using Gymnastika.Tests.Support;
using System;
using System.Data.SqlServerCe;
using System.Transactions;

namespace Gymnastika.Data.Tests.Migration
{
    [TestFixture]
    public class DataMigrationManagerTests
    {
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;

        [SetUp]
        public void SetUp()
        {
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "GymnastikaTest.sdf");
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }

            _container = new UnityContainer();
            _container
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<IDataMigrationManager, StubDataMigrationManager>()
                .RegisterType<IAutomappingConfigurer, MockAutomappingConfigurer>(new PerThreadLifetimeManager())
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IMigrationLoader, MockMigrationLoader>("Default")
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ILogger, FileLogger>()
                .RegisterType<IWorkEnvironment, WorkEnvironment>(new ContainerControlledLifetimeManager())
                .RegisterInstance<ShellSettings>(new ShellSettings());

            _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        [Test]
        public void LoadMigrations()
        {
            StubDataMigrationManager manager = _container.Resolve<IDataMigrationManager>() as StubDataMigrationManager;
            IEnumerable<IDataMigration> dataMigrations = manager.CallLoadDataMigrations();        

            Assert.That(dataMigrations.Count(), Is.EqualTo(1));

            IDataMigration migration = dataMigrations.First();
            Assert.That(migration.Version, Is.EqualTo("00000000000000"));
            Assert.That(migration.TableName, Is.EqualTo("TestTables"));
        }

        [Test]
        public void MigrateToLatestVersion()
        {
            using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
            {
                StubDataMigrationManager manager = _container.Resolve<IDataMigrationManager>() as StubDataMigrationManager;
                manager.Migrate();
            }

            using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
            {
                IRepository<MigrationRecord> repository = _container.Resolve<IRepository<MigrationRecord>>();

                int count = repository.Count(m => true);
                Assert.That(count, Is.EqualTo(2));
            }
        }

        //[Test]
        //public void MigrateToSpecificVersion()
        //{
        //    using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
        //    {
        //        StubDataMigrationManager manager = _container.Resolve<IDataMigrationManager>() as StubDataMigrationManager;
        //        manager.Migrate();
        //    }

        //    using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
        //    {
        //        StubDataMigrationManager manager = _container.Resolve<IDataMigrationManager>() as StubDataMigrationManager;
        //        manager.Migrate("00000000000000");
        //    }

        //    using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
        //    {
        //        ISessionLocator sessionLocator = _container.Resolve<ISessionLocator>();
        //        ISession session = sessionLocator.For(this.GetType());
        //        session.Clear();

        //        int count = session.Linq<MigrationRecord>().Count();
        //        Assert.That(count, Is.EqualTo(1));
        //    }
        //}

        //[Test]
        //public void TransactionScopeComplete()
        //{
        //    string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "TestDb.sdf");
        //    string connectionString = "Data Source=" + dbPath;

        //    if (!File.Exists(dbPath))
        //    {
        //        SqlCeEngine engine = new SqlCeEngine();
        //        engine.LocalConnectionString = connectionString;
        //        engine.CreateDatabase();
        //        engine.Dispose();
        //    }

        //    using (var scope = new TransactionScope())
        //    {
        //        SqlCeConnection conn = new SqlCeConnection(connectionString);
        //        conn.Open();
        //        SqlCeCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = "create table MigrationRecords (Id INT IDENTITY NOT NULL, Version NVARCHAR(16) null, TableName NVARCHAR(255) null, primary key ( Id ) )";
        //        cmd.ExecuteNonQuery();

        //        scope.Complete();
        //    }
        //}
    }

    internal class StubDataMigrationManager : DataMigrationManager
    {
        public StubDataMigrationManager(
            IMigrationLoader[] migrationLoaders,
            ISessionLocator sessionLocator,
            IDataMigrationInterpreter interpreter,
            IRepository<MigrationRecord> repository,
            ILogger logger)
            : base(migrationLoaders, sessionLocator, interpreter, repository, logger)
        {

        }

        public IEnumerable<IDataMigration> CallLoadDataMigrations()
        {
            return base.LoadDataMigrations();
        }
    }
}
