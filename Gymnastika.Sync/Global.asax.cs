using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Configuration;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data;
using Gymnastika.Data.Migration.Generator;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.Providers;
using Gymnastika.Services.Session;
using Gymnastika.Common.Configuration;
using Gymnastika.Common.Logging;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Sync
{
    public class Global : HttpApplication
    {
        private IUnityContainer _container;

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
            InitializeDataService();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(
                new ServiceRoute("registration", new WebServiceHostFactory(), typeof(RegistrationService)));
        }

        private void InitializeDataService()
        {
            _container = new UnityContainer();

            _container
                .RegisterType<ILogger, ConsoleLogger>()
                .RegisterType<IDataMigrationManager, DataMigrationManager>()
                .RegisterType<SchemaBuilder>()
                .RegisterType<IAutomappingConfigurer, FileAutomappingConfigurer>()
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>()
                .RegisterType<IDataMigrationInterpreter, DefaultDataMigrationInterpreter>()
                .RegisterType<ISchemaCommandGenerator, SchemaCommandGenerator>()
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                .RegisterType<IWorkEnvironment, WorkEnvironment>(new ContainerControlledLifetimeManager())
                .RegisterType<ISessionManager, SessionManager>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IUnityContainer>(_container)
                .RegisterInstance<IDataMigrationDiscoverer>(
                    new DataMigrationDiscoverer()
                        .AddFromAssemblyOf(this.GetType())
                        .AddFromAssemblyOf<SchemaBuilder>())
                .RegisterInstance(
                    new ShellSettings
                    {
                        DataProvider = ConfigurationManager.AppSettings["DataProvider"],
                        DatabaseName = ConfigurationManager.AppSettings["DatabaseName"],
                        DataFolder = ConfigurationManager.AppSettings["DataFolder"]
                    });

            IServiceLocator serviceLocator = new UnityServiceLocator(_container);
            _container.RegisterInstance<IServiceLocator>(serviceLocator);

            ServiceLocator.SetLocatorProvider(() => _container.Resolve<IServiceLocator>());
        }
    }
}
