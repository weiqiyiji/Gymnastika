using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using Microsoft.Practices.Unity;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Configuration;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data.Providers;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Migration.Generator;
using Gymnastika.Common.Configuration;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Common.Logging;
using Gymnastika.Data;
using System.IO;

namespace Gymnastika.MigrationGen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //ConfigurationContainers();
        }

        private void ConfigurationContainers()
        {
            _container = new UnityContainer();
            _container
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<IDataMigrationManager, DataMigrationManager>()
                .RegisterType<IAutomappingConfigurer, FileAutomappingConfigurer>(new PerThreadLifetimeManager())
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IMigrationLoader, NullMigrationLoader>("Default")
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ILogger, NullLogger>()
                .RegisterType<IWorkEnvironment, WorkEnvironment>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IUnityContainer>(_container)
                .RegisterInstance<ShellSettings>(
                    new ShellSettings
                    {
                        DatabaseName = "GymnastikaTest.sdf",
                        DataFolder = Directory.GetCurrentDirectory(),
                        DataProvider = "SqlCe",
                    });

            _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
        }
    }
}
