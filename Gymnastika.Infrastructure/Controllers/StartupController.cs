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
                .RegisterType<INavigationManager, NavigationManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserService, UserService>()
                .RegisterType<IStartupView, StartupView>("StartupView", new ContainerControlledLifetimeManager())
                .RegisterType<IMainView, MainView>("MainView", new ContainerControlledLifetimeManager())
                .RegisterType<IUserProfileView, UserProfileView>()
                .RegisterType<StartupViewModel>()
                .RegisterType<MainViewModel>()
                .RegisterType<UserProfileViewModel>();
        }

        protected void RegisterStartupViewWithRegion()
        {
            _startupView = _container.Resolve<IStartupView>("StartupView");

            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, () => _startupView);
            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, () => _container.Resolve<IMainView>("MainView"));
        }

        protected void SubscribeEvents()
        {
            _container
                .Resolve<IEventAggregator>()
                .GetEvent<LogOnCompleteEvent>()
                .Subscribe(OnUserLogOnComplete);
        }

        private void OnUserLogOnComplete(User user)
        {
            if (user != null)
            {
                _container.Resolve<ISessionManager>().Add(user);
                IRegion displayRegion = _regionManager.Regions[RegionNames.DisplayRegion];
                displayRegion.RequestNavigate(new Uri("MainView", UriKind.Relative));
            }
        }

        public void RequestLogOn(string userName)
        {
            OpenProfileWindow(userName, UserProfileViewModel.LogOnTabIndex);
        }

        public void RequestCreateNewUser()
        {
            OpenProfileWindow(string.Empty, UserProfileViewModel.CreateNewUserTabIndex);
        }

        private void OpenProfileWindow(string userName, int initTabIndex)
        {
            IUserProfileView view = _container.Resolve<IUserProfileView>();
            UserProfileViewModel vm = view.Model;
            vm.UserName = userName;
            vm.InitialTabIndex = initTabIndex;
            view.Show();
        }
    }
}
