using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Gymnastika.Common;
using Gymnastika.Controllers;
using Gymnastika.Widgets;
using Gymnastika.Widgets.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Views;
using Microsoft.Practices.Unity;

namespace Gymnastika.ViewModels
{
    public class MainViewModel : NotificationObject, INavigationAware
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IWidgetManager _widgetManager;

        public MainViewModel(
            IUnityContainer container,
            IRegionManager regionManager,
            IWidgetManager widgetManager)
        {
            _container = container;
            _regionManager = regionManager;
            _widgetManager = widgetManager;
        }

        public ObservableCollection<WidgetDescriptor> Widgets
        {
            get { return _widgetManager.Descriptors; }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadModules();
            Initialize();
        }

        private void Initialize()
        {
            ConfigureMainView();
        }

        private void ConfigureMainView()
        {
            _container
                .RegisterType(typeof (WidgetPanelViewModel));
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(DefaultWidgetPanel));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void LoadModules()
        {
            IModuleCatalog moduleCatelog = _container.Resolve<IModuleCatalog>();
            IModuleManager moduleManager = _container.Resolve<IModuleManager>();

            foreach (var moduleInfo in moduleCatelog.Modules)
            {
                moduleManager.LoadModule(moduleInfo.ModuleName);
            }
        }

    }
}
