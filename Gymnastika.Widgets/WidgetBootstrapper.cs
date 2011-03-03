using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Gymnastika.Widgets.Behaviors;
using Gymnastika.Widgets.Views;
using Microsoft.Practices.Unity;

namespace Gymnastika.Widgets
{
    public class WidgetBootstrapper : IWidgetBootstrapper
    {
        private readonly IUnityContainer _container;

        public WidgetBootstrapper(IUnityContainer container)
        {
            _container = container;
        }

        public void Run()
        {
            RegisterDependencies();
            ConfigureContainerAdapterMappings();
            ConfigureBehaviorMappings();
        }

        private void RegisterDependencies()
        {
            _container
                .RegisterType<ContainerAdapterMappings>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainer, WidgetContainer>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetManager, WidgetManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetHost, WidgetHost>()
                .RegisterType<IWidgetContainerAccessor, WidgetContainerAccessor>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetHostFactory, DefaultWidgetHostFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerAdapter, CanvasWidgetContainerAdapter>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerInitializer, WidgetContainerInitializer>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerBehaviorFactory, WidgetContainerBehaviorFactory>(new ContainerControlledLifetimeManager());
        }

        private void ConfigureContainerAdapterMappings()
        {
            ContainerAdapterMappings adapterMappings = _container.Resolve<ContainerAdapterMappings>();
            adapterMappings.RegisterMapping(typeof(Canvas), _container.Resolve<IWidgetContainerAdapter>());
        }
        
        private void ConfigureBehaviorMappings()
        {
            IWidgetContainerBehaviorFactory factory = _container.Resolve<IWidgetContainerBehaviorFactory>();
            factory
                .Register(CreateWidgetHostBehavior.BehaviorKey, typeof (CreateWidgetHostBehavior))
                .Register(DragWidgetBehavior.BehaviorKey, typeof(DragWidgetBehavior));
        }
    }
}
