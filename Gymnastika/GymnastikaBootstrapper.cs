using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Gymnastika.Views;
using Gymnastika.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using Gymnastika.Common.Services;
using Gymnastika.Controllers;

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
                .RegisterType<IUserService, UserService>()
                .RegisterType<IStartupController, StartupController>();


            base.ConfigureContainer();
        }
    }
}
