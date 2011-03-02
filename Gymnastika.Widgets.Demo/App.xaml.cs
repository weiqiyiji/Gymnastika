using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gymnastika.Widgets.Behaviors;
using Gymnastika.Widgets.Views;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Gymnastika.Widgets.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;
        private IServiceLocator _serviceLocator;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new UnityContainer();
            _container
                .RegisterInstance(_container)
                .RegisterType<ContainerAdapterMappings>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainer, WidgetContainer>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetManager, WidgetManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetHost, WidgetHost>()
                .RegisterType<IWidgetContainerAccessor, WidgetContainerAccessor>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetHostFactory, DefaultWidgetHostFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerAdapter, CanvasWidgetContainerAdapter>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerInitializer, WidgetContainerInitializer>(new ContainerControlledLifetimeManager())
                .RegisterType<IWidgetContainerBehaviorFactory, WidgetContainerBehaviorFactory>(new ContainerControlledLifetimeManager());

            _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
            _container.RegisterInstance<IServiceLocator>(_serviceLocator);

            ConfigureBehaviorMappings();
            ConfigureContainerAdapterMappings();
        }

        private void ConfigureContainerAdapterMappings()
        {
            ContainerAdapterMappings adapterMappings = _container.Resolve<ContainerAdapterMappings>();
            adapterMappings.RegisterMapping(typeof(Canvas), _container.Resolve<IWidgetContainerAdapter>());
        }

        private void ConfigureBehaviorMappings()
        {
            IWidgetContainerBehaviorFactory factory = _container.Resolve<IWidgetContainerBehaviorFactory>();
            factory.Register(CreateWidgetHostBehavior.BehaviorKey, typeof(CreateWidgetHostBehavior));
        }
    }
}
