using System.Configuration;
using System.IO;
using System.Windows;
using Gymnastika.Common.Configuration;
using Gymnastika.Common.Logging;
using Gymnastika.Controllers;
using Gymnastika.Data;
using Gymnastika.Data.Configuration;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Migration.Generator;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Views;
using Gymnastika.Widgets;
using Gymnastika.Widgets.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

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
            IWidgetBootstrapper widgetBootstrapper = Container.Resolve<IWidgetBootstrapper>();
            widgetBootstrapper.Run();

            IStartupController controller = Container.Resolve<IStartupController>();
            controller.Run();

            Application.Current.MainWindow = this.Shell as Shell;
            Application.Current.MainWindow.Show();
        }

        private void MigrateData()
        {
            using (IWorkContextScope scope = Container.Resolve<IWorkEnvironment>().GetWorkContextScope())
            {
                IDataMigrationManager manager = Container.Resolve<IDataMigrationManager>();
                manager.Migrate();
            }
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        protected override void ConfigureContainer()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            Container
                .RegisterType<Shell>()
                .RegisterType<ILogger, ConsoleLogger>()
                .RegisterType<IStartupController, StartupController>()
                .RegisterType<IMainController, MainController>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataMigrationManager, DataMigrationManager>()
                .RegisterType<SchemaBuilder>()
                .RegisterType<IAutomappingConfigurer, FileAutomappingConfigurer>()
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>()
                .RegisterType<ISessionLocator, SessionLocator>()
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ISchemaCommandGenerator, SchemaCommandGenerator>()
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType(typeof (IRepository<>), typeof (Repository<>))
                .RegisterType<ILogger, ConsoleLogger>()
                .RegisterType<IWidgetBootstrapper, WidgetBootstrapper>()
                .RegisterType<IWorkEnvironment, WorkEnvironment>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IUnityContainer>(Container)
                .RegisterInstance<IDataMigrationDiscoverer>(
                    new DataMigrationDiscoverer()
                        .AddFromDirectory(currentDirectory, x => x.Contains("Gymnastika.Modules."))
                        .AddFromDirectory(currentDirectory, x => x.Contains("Gymnastika.Services"))
                        .AddFromAssemblyOf<SchemaBuilder>()
                );

            var shellSettings = new ShellSettings
            {
                DataProvider = ConfigurationManager.AppSettings["DataProvider"],
                DatabaseName = ConfigurationManager.AppSettings["DatabaseName"],
                DataFolder = ConfigurationManager.AppSettings["DataFolder"]
            };

            Container.RegisterInstance(shellSettings);
            base.ConfigureContainer();
        }
    }
}
