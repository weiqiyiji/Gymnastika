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

using Gymnastika.Services;
using Microsoft.Practices.Prism.Modularity;
using Gymnastika.Services.Models;
using Gymnastika.Services.Session;
using Gymnastika.Services.Impl;
using Gymnastika.Services.Contracts;
using Gymnastika.Events;

namespace Gymnastika.Controllers
{
    public class StartupController : IStartupController
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;
        private IStartupView _startupView;

        public StartupController(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Run()
        {
            RegisterDependencies();
            RegisterStartupViewWithRegion();
            SubscribeEvents();
        }

        protected void RegisterDependencies()
        {
            _container
                .RegisterType<IUserService, UserService>()
                .RegisterType<ISessionManager, SessionManager>(new ContainerControlledLifetimeManager());

            //Register Views
            _container
                .RegisterType<IStartupView, StartupView>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainView, MainView>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogOnView, LogOnView>()
                .RegisterType<ICreateNewUserView, CreateNewUserView>();

            //Register ViewModels
            _container
                .RegisterType<LogOnViewModel>()
                .RegisterType<StartupViewModel>()
                .RegisterType<MainViewModel>()
                .RegisterType<CreateNewUserViewModel>();
        }

        protected void RegisterStartupViewWithRegion()
        {
            _startupView = _container.Resolve<IStartupView>();

            _regionManager.RegisterViewWithRegion(
                    RegionNames.DisplayRegion, () => _startupView);
        }

        protected void SubscribeEvents()
        {
            _container
                .Resolve<IEventAggregator>()
                .GetEvent<LogOnSuccessEvent>()
                .Subscribe(OnUserLogOnSuccess);
        }

        private void OnUserLogOnSuccess(User user)
        {
            IMainView view = _container.Resolve<IMainView>();
            IRegion displayRegion = _regionManager.Regions[RegionNames.DisplayRegion];

            displayRegion.Remove(_startupView);
            displayRegion.Add(view);
            displayRegion.Activate(view);

            LoadModules();
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

        public void RequestLogOn(string userName)
        {
            ILogOnView view = _container.Resolve<ILogOnView>();
            LogOnViewModel vm = view.Model;
            vm.UserName = userName;
            
            view.Show();
        }
    }
}
