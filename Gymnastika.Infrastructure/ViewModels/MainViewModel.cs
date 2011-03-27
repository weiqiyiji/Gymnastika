using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Gymnastika.Common;
using Gymnastika.Controllers;
using Gymnastika.Widgets;
using Gymnastika.Widgets.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Views;
using Microsoft.Practices.Unity;
using Gymnastika.Common.Navigation;

namespace Gymnastika.ViewModels
{
    public class MainViewModel : NotificationObject, INavigationAware
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IWidgetManager _widgetManager;
        private readonly INavigationManager _navigationManager;
        private readonly INavigationService _navigationService;

        public MainViewModel(
            IUnityContainer container,
            IRegionManager regionManager,
            IWidgetManager widgetManager,
            INavigationManager navigationManager,
            INavigationService navigationService)
        {
            _container = container;
            _regionManager = regionManager;
            _widgetManager = widgetManager;
            _navigationManager = navigationManager;
            _navigationService = navigationService;
        }

        public ObservableCollection<WidgetDescriptor> Widgets
        {
            get { return _widgetManager.Descriptors; }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsInitializing = true;
            LoadModules();
            Initialize();
            IsInitializing = false;
        }
        
        private bool _isInitializing;

        public bool IsInitializing
        {
            get { return _isInitializing; }
            set
            {
                if (_isInitializing != value)
                {
                    _isInitializing = value;
                    RaisePropertyChanged("IsInitializing");
                }
            }
        }

        private const string NormalRegion = "NormalRegion";
        private const string RegionHeader = "常规";

        private void Initialize()
        {
            ConfigureMainView();

            _navigationManager.AddRegionIfMissing(NormalRegion, RegionHeader);

            INavigationRegion normalRegion = _navigationManager.Regions[NormalRegion];

            normalRegion.Add(
                new NavigationDescriptor()
                    {
                        ViewName = "WidgetView",
                        Header = "主 页",
                        ViewResolver = () => _container.Resolve<DefaultWidgetPanel>()
                    });

            normalRegion.Add(
                 new NavigationDescriptor()
                 {
                     ViewName = "UserProfileManagementView",
                     Header = "账户管理",
                     ViewResolver = () => _container.Resolve<UserProfileManagementView>()
                 });

            normalRegion.Add(
                new NavigationDescriptor() 
                { 
                    ViewName = "SyncView",
                    Header = "连接手机",
                    ViewResolver = () => _container.Resolve<SyncView>()
                });

            _navigationService.RequestNavigate(NormalRegion, "WidgetView");
        }
				
        private void ConfigureMainView()
        {
            _container
                .RegisterType<UserProfileManagementView>(new ContainerControlledLifetimeManager())
                .RegisterType<DefaultWidgetPanel>(new ContainerControlledLifetimeManager())
                .RegisterType<WidgetPanelViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<NavigationView>(new ContainerControlledLifetimeManager())
                .RegisterType<SyncView>(new ContainerControlledLifetimeManager());

            _regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof (NavigationView));
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

            foreach (var moduleInfo in moduleCatelog.Modules.Where(m => m.InitializationMode == InitializationMode.OnDemand))
            {
                moduleManager.LoadModule(moduleInfo.ModuleName);
            }
        }
    }
}
