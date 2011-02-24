using Gymnastika.Common;
using Gymnastika.Events;
using Gymnastika.Services.Contracts;
using Gymnastika.Services.Impl;
using Gymnastika.Services.Models;
using Gymnastika.Services.Session;
using Gymnastika.ViewModels;
using Gymnastika.Views;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

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
                .RegisterType<IUserProfileView, UserProfileView>();

            //Register ViewModels
            _container
                .RegisterType<StartupViewModel>()
                .RegisterType<MainViewModel>()
                .RegisterType<UserProfileViewModel>();
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
            IUserProfileView view = _container.Resolve<IUserProfileView>();
            UserProfileViewModel vm = view.Model;
            vm.InitialTabIndex = UserProfileViewModel.LogOnTabIndex;
            vm.UserName = userName;
            view.Show();
        }

        public void RequestCreateNewUser()
        {
            IUserProfileView view = _container.Resolve<IUserProfileView>();
            UserProfileViewModel vm = view.Model;
            vm.InitialTabIndex = UserProfileViewModel.CreateNewUserTabIndex;
            view.Show();
        }
    }
}
