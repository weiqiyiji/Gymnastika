using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Services;
using Gymnastika.Modules.Sports.Views;
using Gymnastika.Common;

namespace Gymnastika.Modules.Sports
{
    public class SportsManagementModule : IModule
    {
        readonly private IRegionManager _regionManager;
        readonly private IUnityContainer _container;

        public SportsManagementModule(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
            Initialize();
        }


        #region IModule Members

        public void Initialize()
        {
            RegisterDependencies();
            RegisterRegions();
        }

        private void RegisterRegions()
        {
            _regionManager
                .RegisterViewWithRegion(RegionNames.DisplayRegion,typeof(ICategoriesPanelView));
        }

        #endregion


        #region RegisterDependencies

        private void RegisterDependencies()
        {
            _container
                .RegisterType<ICategoriesProvider, CategoriesProvider>(new ContainerControlledLifetimeManager())

                .RegisterType<ICategoriesPanelViewModel, CategoriesPanelViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPanelViewModel, SportsPanelViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPlanViewModel,SportsPlanViewModel>(new ContainerControlledLifetimeManager())
                

                .RegisterType<ISportsPanelView, SportsPanelView>(new ContainerControlledLifetimeManager())
                .RegisterType<ICategoriesPanelView, CategoriesPanelView>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPlanView, SportsPlanView>(new ContainerControlledLifetimeManager());
                
        }

        #endregion
    
    }
}
