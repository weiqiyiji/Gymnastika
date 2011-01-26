using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Gymnastika.Views;
using Gymnastika.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using Gymnastika.Common.UserManagement;
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
            
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri("/Gymnastika;Component/Data/ModulesCatalog.xaml"));
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
