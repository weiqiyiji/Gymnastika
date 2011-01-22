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
using Gymnastika.UserManagement;

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
            IRegionManager regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, () => Container.Resolve<StartupView>());
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri("/Gymnastika.Desktop;Component/ModulesCatalog.xaml"));
        }
        
        protected override void ConfigureContainer()
        {
            //Register Views
            Container
                .RegisterType<MainView>()
                .RegisterType<StartupView>()
                .RegisterType<Shell>();

            //Register ViewModels
            Container
                .RegisterType<StartupViewModel>();

            Container
                .RegisterType<IUserService, UserService>();

            base.ConfigureContainer();
        }
    }
}
