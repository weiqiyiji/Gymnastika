using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Gymnastika.Views;
using Gymnastika.ViewModels;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Common.Events;
using Gymnastika.Common.UserManagement;

namespace Gymnastika.Controllers
{
    public class StartupController
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;

        public StartupController(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Run()
        {
            RegisterDependencies();
            RegisterViewsWithRegions();
            SubscribeEvents();
        }

        protected void RegisterDependencies()
        { 
            //Register Views
            _container
                .RegisterType<IStartupView, StartupView>()
                .RegisterType<IMainView, MainView>()
                .RegisterType<Shell>();

            //Register ViewModels
            _container
                .RegisterType<StartupViewModel>();
        }

        protected void RegisterViewsWithRegions()
        {
            _regionManager.RegisterViewWithRegion(
                    RegionNames.DisplayRegion, 
                    () => _container.Resolve<StartupViewModel>().View);
        }

        protected void SubscribeEvents()
        {
            _container
                .Resolve<IEventAggregator>()
                .GetEvent<LogOnSuccessEvent>()
                .Subscribe(ProcessUserLogOnSuccess);
        }

        private void ProcessUserLogOnSuccess(User user)
        {
            var view = _container.Resolve<MainViewModel>().View;
            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, () => view);
            _regionManager.Regions[RegionNames.DisplayRegion].Activate(view);
        }
    }
}
