using System;
using System.Configuration;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Gymnastika.Common.Configuration;
using Gymnastika.Common.Logging;
using Gymnastika.Data;
using Gymnastika.Data.Configuration;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Migration.Generator;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Services.Session;
using Gymnastika.Sync.Infrastructure;
using Gymnastika.Sync.Schedule;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Gymnastika.Sync
{
    public class Global : HttpApplication
    {
        private IUnityContainer _container;

        void Application_Start(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
            log4net.Config.XmlConfigurator.Configure();
            RegisterRoutes();
            InitializeDataService();
            MigrateData();
        }

        private void MigrateData()
        {
            using (IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().GetWorkContextScope())
            {
                IDataMigrationManager manager = _container.Resolve<IDataMigrationManager>();
                manager.Migrate();
            }
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("", new UnityWebServiceHostFactory(), typeof(RegistrationService)));
            RouteTable.Routes.Add(new ServiceRoute("schedule", new UnityWebServiceHostFactory(), typeof(ScheduleService)));
        }

        private void InitializeDataService()
        {
            _container = new UnityContainer();

            _container
                .RegisterType<ILogger, ConsoleLogger>()
                .RegisterType<IDataMigrationManager, DataMigrationManager>()
                .RegisterType<SchemaBuilder>()
                .RegisterType<IAutomappingConfigurer, WcfAutomappingConfigurer>()
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataServicesProviderFactory, CustomSqlCeDataServicesProviderFactory>()
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ISchemaCommandGenerator, SchemaCommandGenerator>()
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                .RegisterType<IWorkEnvironment, WebEnvironment>(new ContainerControlledLifetimeManager())
                .RegisterType<ISessionManager, SessionManager>(new ContainerControlledLifetimeManager())
                .RegisterType<RemindManager>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IUnityContainer>(_container)
                .RegisterInstance<IDataMigrationDiscoverer>(
                    new DataMigrationDiscoverer()
                        .AddFromAssemblyOf<RegistrationService>()
                        .AddFromAssemblyOf<SchemaBuilder>())
                .RegisterInstance(
                    new ShellSettings
                    {
                        DataProvider = ConfigurationManager.AppSettings["DataProvider"],
                        DatabaseName = ConfigurationManager.AppSettings["DatabaseName"],
                        DataFolder = Server.MapPath("~/" + ConfigurationManager.AppSettings["DataFolder"])
                    });

            IServiceLocator serviceLocator = new UnityServiceLocator(_container);
            _container.RegisterInstance<IServiceLocator>(serviceLocator);

            ServiceLocator.SetLocatorProvider(() => _container.Resolve<IServiceLocator>());
        }
    }
}
