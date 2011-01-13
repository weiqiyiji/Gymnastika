using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;

namespace Gymnastika.Desktop
{
    public class GymnastikaBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri("/Gymnastika.Desktop;Component/ModulesCatalog.xaml"));
        }
        
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }
    }
}
