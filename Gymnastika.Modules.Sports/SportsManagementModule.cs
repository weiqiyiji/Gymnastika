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
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Data;
using Gymnastika.Widgets;

namespace Gymnastika.Modules.Sports
{
    public class SportsManagementModule : IModule
    {
        readonly private IRegionManager _regionManager;
        readonly private IUnityContainer _container;
        readonly private IWidgetManager _widgetManager;
        public SportsManagementModule(IUnityContainer container, IRegionManager regionManager,IWidgetManager widgetManager)
        {
            _regionManager = regionManager;
            _container = container;
            _widgetManager = widgetManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterDependencies();
            ConfigImporters();
            ImportData();
            RegisterWidgets();
            RegisterViews();
        }

        private void RegisterViews()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.DisplayRegion, typeof(ModuleShell));
            _regionManager.RegisterViewWithRegion(ModuleRegionNames.CategoryRegion, typeof(ICategoriesPanelView))
                          .RegisterViewWithRegion(ModuleRegionNames.PlanRegion, typeof(ISportsPlanView))
                          .RegisterViewWithRegion(ModuleRegionNames.SportRegion, typeof(ISportsPanelView));
        }

        #endregion


        private void RegisterWidgets()
        {

        }

        private void ImportData()
        {
            var manager = _container.Resolve<IDataImportManager>();
            manager.ImportData();
        }

        private void ConfigImporters()
        {
            IImporterCollection collection = _container.Resolve<IImporterCollection>();
            collection.Add(
                new CategoryImporter 
                    (new XmlCategorySource(@"data/sports/SportData.xml"),
                    _container.Resolve<IRepository<SportsCategory>>(),
                    _container.Resolve<IRepository<Sport>>(),
                    _container.Resolve<IWorkEnvironment>()));
        }



        #region RegisterDependencies

        private void RegisterDependencies()
        {

            //Dependency
            _container
                //Data
                .RegisterType<IDataImportManager,DataImportManager>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IImporterCollection>(new ImporterCollection())

                //Services
                .RegisterType<ICategoriesProvider, CategoriesProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPlanProvider,SportsPlanProvider>(new ContainerControlledLifetimeManager())
                .RegisterInstance<ISportsPlanItemViewModelFactory>(new SportsPlanItemViewModelFactory())
                .RegisterInstance<ISportCardViewModelFactory>(new SportCardViewModelFactory())
                
                //ViewModels
                .RegisterType<ICategoriesPanelViewModel, CategoriesPanelViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPanelViewModel, SportsPanelViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPlanViewModel, SportsPlanViewModel>(new ContainerControlledLifetimeManager())

                //Views
                .RegisterType<ISportsPanelView, SportsPanelView>(new ContainerControlledLifetimeManager())
                .RegisterType<ICategoriesPanelView, CategoriesPanelView>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportsPlanView, SportsPlanView>(new ContainerControlledLifetimeManager());
        }

        #endregion
    
    }
}
