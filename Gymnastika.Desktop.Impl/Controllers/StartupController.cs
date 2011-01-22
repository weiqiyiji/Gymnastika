using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Gymnastika.Desktop.Views;
using Gymnastika.Desktop.ViewModels;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Desktop.Core;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Desktop.Core.Events;
using Gymnastika.Desktop.Core.UserManagement;

namespace Gymnastika.Desktop.Controllers
{
    public class StartupController
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        public StartupController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public void Run()
        {
            RegisterDependencies();
            RegisterViewsWithRegions();
            SubscribeEvents();
        }

        private void RegisterDependencies()
        { 
            //Register Views
            _container
                .RegisterType<StartupView>()
                .RegisterType<MainView>()
                .RegisterType<Shell>();

            //Register ViewModels
            _container
                .RegisterType<StartupViewModel>();
        }

        private void RegisterViewsWithRegions()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, 
                () => _container.Resolve<StartupViewModel>().View);
        }

        private void SubscribeEvents()
        {
            _eventAggregator.GetEvent<LogOnSuccessEvent>().Subscribe(ProcessUserLogOnSuccess);
        }

        private void ProcessUserLogOnSuccess(User user)
        { 
            
        }
    }
}
