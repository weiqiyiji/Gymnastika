using System;
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
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private IStartupView _startupView;

        public StartupController(
            IUnityContainer container, IRegionManager regionManager)
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
                .RegisterType<ISessionManager, SessionManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IStartupView, StartupView>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainView, MainView>("MainView", new ContainerControlledLifetimeManager())
                .RegisterType<IUserProfileView, UserProfileView>()
                .RegisterType<StartupViewModel>()
                .RegisterType<MainViewModel>()
                .RegisterType<UserProfileViewModel>();
        }

        protected void RegisterStartupViewWithRegion()
        {
            _startupView = _container.Resolve<IStartupView>();

            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, () => _startupView);
            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, () => _container.Resolve<IMainView>("MainView"));
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
            _container.Resolve<ISessionManager>().Add(user);
            IRegion displayRegion = _regionManager.Regions[RegionNames.DisplayRegion];
            displayRegion.RequestNavigate(new Uri("MainView", UriKind.Relative)); 
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
