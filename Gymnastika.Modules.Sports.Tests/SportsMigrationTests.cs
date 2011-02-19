using Gymnastika.Modules.Sports.Views;
using System;
using Gymnastika.Modules.Sports.Services;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Modules.Sports.Models;
using System.ComponentModel;
using NUnit.Framework;
using Moq;
using Gymnastika.Modules.Sports.ViewModels;
using System.IO;
using Microsoft.Practices.Unity;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Configuration;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Common.Logging;
using Gymnastika.Tests.Support;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Common.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Gymnastika.Data.Models;
using NHibernate;
using Gymnastika.Data.Tests.MockMigration;

namespace Gymnastika.Modules.Sports.Tests
{
    public class MockAutomappingConfigurer : IAutomappingConfigurer
    {
        #region IAutomappingConfigurer Members

        public System.Collections.Generic.IEnumerable<AutomappingConfigurationMetadata> GetAutomappingMetadata()
        {
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Data.Tests.dll" };
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Data.dll" };
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Modules.Sports.Tests.dll" };
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Modules.Sports.dll" };
        }

        #endregion
    }

    public class MockMigrationLoader : IMigrationLoader
    {
        #region IMigrationLoader Members

        public IEnumerable<IDataMigration> Load()
        {
           return new List<IDataMigration>()
           {
               new  Migration_AnotherTestTables_00000000000001()
           };
            //return Assembly.GetAssembly(typeof(Sport))
            //            .GetExportedTypes()
            //            .Where(t => t.GetInterface("Gymnastika.Data.Migration.IDataMigration") != null)
            //            .Select(t => (IDataMigration)Activator.CreateInstance(t));
        }

        #endregion
    }

    [TestFixture]
    public class SportsMigrationTests
    {
        private readonly string DbName = "GymnastikaForTests.sdf";
        private readonly string DbFolder = Directory.GetCurrentDirectory();
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;

        [SetUp]
        public void SetUp()
        {
            string dbPath = Path.Combine(DbFolder, DbName);
            if (File.Exists(dbPath)) File.Delete(dbPath);

            _container = new UnityContainer();
            _container
               .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>(new ContainerControlledLifetimeManager())
               .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
               .RegisterType<ITransactionManager, TransactionManager>()
               .RegisterType<IDataMigrationManager, DataMigrationManager>()
               .RegisterType<IAutomappingConfigurer, MockAutomappingConfigurer>(new PerThreadLifetimeManager())
               .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
               .RegisterInstance<IMigrationLoader[]>(
               new IMigrationLoader[]
               {
                    new MockMigrationLoader()
               })
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
        public void Test()
        {
            IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().GetWorkContextScope();
            var migrationManager = _container.Resolve<IDataMigrationManager>();
            migrationManager.Migrate();
            Repository<Sport> re = new Repository<Sport>(_container.Resolve<ISessionLocator>(), _container.Resolve<ILogger>());
            scope.Dispose();
        }
    }
}
