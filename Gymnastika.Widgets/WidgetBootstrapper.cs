using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Gymnastika.Widgets.Behaviors;
using Gymnastika.Widgets.Views;
using Microsoft.Practices.Unity;
using Microsoft.Surface.Presentation.Controls;

namespace Gymnastika.Widgets
{
    public class WidgetBootstrapper : IWidgetBootstrapper
    {
        protected IUnityContainer Container { get; set; }

        public WidgetBootstrapper(IUnityContainer container)
        {
            Container = container;
        }

        public void Run()
        {
            RegisterDependencies();
            ConfigureContainerAdapterMappings();
            ConfigureBehaviorMappings();
        }

        protected virtual void RegisterDependencies()
        {
            Container
                .RegisterType<ContainerAdapterMappings>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainer, WidgetContainer>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetManager, WidgetManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetHost, WidgetHost>()
                .RegisterType<IWidgetContainerAccessor, WidgetContainerAccessor>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetHostFactory, DefaultWidgetHostFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerAdapter, CanvasWidgetContainerAdapter>("Canvas", new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerInitializer, WidgetContainerInitializer>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerBehaviorFactory, WidgetContainerBehaviorFactory>(new ContainerControlledLifetimeManager());
        }

        protected virtual void ConfigureContainerAdapterMappings()
        {
            ContainerAdapterMappings adapterMappings = Container.Resolve<ContainerAdapterMappings>();
            adapterMappings.RegisterMapping(typeof(Canvas), Container.Resolve<IWidgetContainerAdapter>("Canvas"));
        }
        
        protected virtual void ConfigureBehaviorMappings()
        {
            IWidgetContainerBehaviorFactory factory = Container.Resolve<IWidgetContainerBehaviorFactory>();
            factory
                .Register(CreateWidgetHostBehavior.BehaviorKey, typeof (CreateWidgetHostBehavior))
                .Register(DragWidgetBehavior.BehaviorKey, typeof(DragWidgetBehavior))
                .Register(DelayCreateWidgetBehavior.BehaviorKey, typeof(DelayCreateWidgetBehavior))
                .Register(SaveWidgetStateBehavior.BehaviorKey, typeof(SaveWidgetStateBehavior));
        }
    }
}
