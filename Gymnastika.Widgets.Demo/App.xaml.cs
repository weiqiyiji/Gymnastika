using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
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
                .RegisterType<IWidgetContainer, WidgetContainer>()
                .RegisterType<IWidgetManager, WidgetManager>()
                .RegisterType<IWidgetHost, WidgetHost>()
                .RegisterType<IWidgetContainerAccessor, WidgetContainerAccessor>()
                .RegisterType<IWidgetHostFactory, DefaultWidgetHostFactory>()
                .RegisterType<IWidgetContainerAdapter, CanvasWidgetContainerAdapter>()
                .RegisterType<IWidgetContainerInitializer, WidgetContainerInitializer>()
                .RegisterType<IWidgetContainerBehaviorFactory, WidgetContainerBehaviorFactory>();

            _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
        }
    }
}
