using System;
using System.Windows;
using Gymnastika.Controllers;
using Gymnastika.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

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
            IStartupController controller = Container.Resolve<IStartupController>();
            controller.Run();

            Application.Current.MainWindow = this.Shell as Shell;
            Application.Current.MainWindow.Show();
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
                .RegisterType<IStartupController, StartupController>(new ContainerControlledLifetimeManager());

            base.ConfigureContainer();
        }
    }
}
