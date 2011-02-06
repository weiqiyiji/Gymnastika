using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows;
using Gymnastika.Modules.Sports.Views;
using Gymnastika.Modules.Sports.Services;

namespace Gymnastika.Modules.Sports
{
    public class SportsManagementModule : IModule
    {
        readonly private IRegionManager _regionManager;
        readonly private IUnityContainer _container;

        public SportsManagementModule(IUnityContainer container, IRegionManager regionManager)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }
            _regionManager = regionManager;
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterDependencies();
        }

        #region RegisterDependencies

        private void RegisterDependencies()
        {
            RegisterServices();
            RegisterViewModels();
            RegisterViews();
        }

        private void RegisterServices()
        {
            _container
                    .RegisterInstance(typeof(ISportsProvider), new SportsProvider())
                    .RegisterInstance(typeof(ISportsPlanProvider), new SportsPlanProvider());
        }

        private void RegisterViewModels()
        {
            _container
                .RegisterType<ISportsListViewModel, SportsListViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPlanViewModel, SportsPlanViewModel>(new ContainerControlledLifetimeManager());
        }

        private void RegisterViews()
        {
            _container
                .RegisterType<ISportView, SportView>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsListView,SportsListView>(new ContainerControlledLifetimeManager());
        }

        #endregion

        #endregion

    }
}
