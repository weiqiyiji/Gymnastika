using System;
using System.Windows;
using Gymnastika.Controllers;
using Gymnastika.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Migration.Generator;
using Gymnastika.Data;
using Gymnastika.Common.Configuration;
using System.Configuration;
using Gymnastika.Common.Logging;
using System.Collections.Generic;
using Gymnastika.Data.Configuration;

namespace Gymnastika
{
    public class GymnastikaBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            MigrateData();

            IStartupController controller = Container.Resolve<IStartupController>();
            controller.Run();

            Application.Current.MainWindow = this.Shell as Shell;
            Application.Current.MainWindow.Show();
        }

        private void MigrateData()
        {
            IDataMigrationManager manager = Container.Resolve<IDataMigrationManager>();
            manager.Migrate();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override IUnityContainer CreateContainer()
        {
            return new UnityContainer().LoadConfiguration();
        }

        protected override void ConfigureContainer()
        {
            Container
                .RegisterType<Shell>()
                .RegisterType<ILogger, ConsoleLogger>()
                .RegisterType<IStartupController, StartupController>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataMigrationFinder, DataMigrationInModuleFinder>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataMigrationManager, DataMigrationManager>(new ContainerControlledLifetimeManager())
                .RegisterType<SchemaBuilder>()
                .RegisterType<IAutomappingConfigurer, FileAutomappingConfigurer>()
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>()
                .RegisterType<ISessionLocator, SessionLocator>()
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ISchemaCommandGenerator, SchemaCommandGenerator>();

            var shellSettings = new ShellSettings
            {
                DataProvider = ConfigurationSettings.AppSettings["DataProvider"],
                DataConnectionString = ConfigurationSettings.AppSettings["DataServiceConnectionString"]
            };

            Container
                .RegisterInstance(shellSettings);

            base.ConfigureContainer();
        }
    }
}
