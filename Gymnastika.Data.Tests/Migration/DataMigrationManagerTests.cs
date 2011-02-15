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
using Moq;

namespace Gymnastika.Data.Tests.Migration
{
    [TestFixture]
    public class DataMigrationManagerTests
    {
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;
        private readonly string DbName = "GymnastikaForTests.sdf";
        private readonly string DbFolder = Directory.GetCurrentDirectory();

        [SetUp]
        public void SetUp()
        {
            string dbPath = Path.Combine(DbFolder, DbName);
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
                .RegisterInstance<ShellSettings>(
                    new ShellSettings 
                    {
                        DatabaseName = DbName,
                        DataFolder = DbFolder,
                        DataProvider = "SqlCe"
                    });

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
            var mockMigrationLoader = new MockMigrationLoader();
            var mockSession = new Mock<ISession>();
            var mockSessionLocator = new Mock<ISessionLocator>();
            mockSessionLocator
                .Setup(s => s.For(It.IsAny<Type>()))
                .Returns(mockSession.Object);

            StubDataMigrationManager manager = new StubDataMigrationManager(
                new IMigrationLoader[] { mockMigrationLoader },
                mockSessionLocator.Object,
                null,
                new InMemoryRepository<MigrationRecord>(),
                new NullLogger()
                );

            IEnumerable<IDataMigration> dataMigrations = manager.CallLoadDataMigrations();        

            Assert.That(dataMigrations.Count(), Is.EqualTo(mockMigrationLoader.MigrationCount));
        }

        [Test]
        public void TestWhat()
        {
            using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().GetWorkContextScope())
            {
                StubDataMigrationManager migrationManager = _container.Resolve<IDataMigrationManager>() as StubDataMigrationManager;
                migrationManager.Migrate();
            }
        }

        //[Test]
        //public void MigrateToLatestVersion()
        //{
        //    using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
        //    {
        //        StubDataMigrationManager manager = _container.Resolve<IDataMigrationManager>() as StubDataMigrationManager;
        //        manager.Migrate();
        //    }

        //    using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().CreateWorkContextScope())
        //    {
        //        IRepository<MigrationRecord> repository = _container.Resolve<IRepository<MigrationRecord>>();

        //        int count = repository.Count(m => true);
        //        Assert.That(count, Is.EqualTo(2));
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
