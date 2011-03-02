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
            _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
            _container.RegisterInstance<IServiceLocator>(_serviceLocator);
            _container
                .RegisterType<IWidgetBootstrapper, WidgetBootstrapper>(new ContainerControlledLifetimeManager())
                .RegisterInstance(_container);

            IWidgetBootstrapper bootstrapper = _container.Resolve<IWidgetBootstrapper>();
            bootstrapper.Run();
        }
    }
}
