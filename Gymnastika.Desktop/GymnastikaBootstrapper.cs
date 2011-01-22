using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Gymnastika.Desktop.Views;
using Gymnastika.Desktop.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Desktop.Core;
using Gymnastika.Desktop.Core.UserManagement;
using Gymnastika.Desktop.Controllers;

namespace Gymnastika.Desktop
{
    public class GymnastikaBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri("/Gymnastika.Desktop;Component/ModulesCatalog.xaml"));
        }
        
        protected override void ConfigureContainer()
        {
            Container
                .RegisterType<IUserService, UserService>()
                .RegisterType<StartupController>();

            base.ConfigureContainer();
        }
    }
}
