using System.IO;
using Gymnastika.Common.Configuration;
using Gymnastika.Common.Logging;
using Gymnastika.Data.Configuration;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data.Tests.Mocks;
using Gymnastika.Tests.Support;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Gymnastika.Data.Tests.Migration
{
    [TestFixture]
    public class DataMigrationManagerTests
    {
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;
        private const string DbName = "GymnastikaForTests.sdf";
        private readonly string _dbFolder = Directory.GetCurrentDirectory();

        [SetUp]
        public void SetUp()
        {
            string dbPath = Path.Combine(_dbFolder, DbName);
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }

            _container = new UnityContainer();
            _container
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<IDataMigrationManager, DataMigrationManager>()
                .RegisterType<IAutomappingConfigurer, MockAutomappingConfigurer>(new PerThreadLifetimeManager())
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ILogger, FileLogger>()
                .RegisterType<IWorkEnvironment, WorkEnvironment>(new ContainerControlledLifetimeManager())
                .RegisterInstance(new DataMigrationDiscoverer().AddFromAssemblyOf<DataMigrationManagerTests>())
                .RegisterInstance(
                    new ShellSettings 
                    {
                        DatabaseName = DbName,
                        DataFolder = _dbFolder,
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
        public void TestWhat()
        {
            using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().GetWorkContextScope())
            {
                IDataMigrationManager migrationManager = _container.Resolve<IDataMigrationManager>();
                migrationManager.Migrate();
            }
        }
    }
}
