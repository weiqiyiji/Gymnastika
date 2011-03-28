using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Practices.Unity;

namespace Gymnastika.Widgets
{
    public class GymnastikaWidgetBootstrapper : WidgetBootstrapper
    {
        public GymnastikaWidgetBootstrapper(IUnityContainer container) : base(container) { }

        protected override void RegisterDependencies()
        {
            base.RegisterDependencies();
            Container.RegisterType<IWidgetContainerAdapter, ScatterViewWidgetContainerAdapter>(
                "ScatterView", new ContainerControlledLifetimeManager());
        }

        protected override void ConfigureContainerAdapterMappings()
        {
            base.ConfigureContainerAdapterMappings();
            ContainerAdapterMappings adapterMappings = Container.Resolve<ContainerAdapterMappings>();
            adapterMappings.RegisterMapping(typeof(ScatterView), Container.Resolve<IWidgetContainerAdapter>("ScatterView"));
        }
    }
}
