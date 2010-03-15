﻿using System;
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
using Gymnastika.Widgets;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Modules.Sports.Services.Factories;
using Gymnastika.Modules.Sports.DataImport.Importers;
using Gymnastika.Modules.Sports.DataImport.Sources;
using Gymnastika.Modules.Sports.DataImport;
using Gymnastika.Modules.Sports.Widget;
using Gymnastika.Common.Navigation;

namespace Gymnastika.Modules.Sports
{
    public class SportsManagementModule : IModule
    {
        readonly private IRegionManager _regionManager;
        readonly private IUnityContainer _container;
        readonly private IWidgetManager _widgetManager;
        readonly private INavigationManager _navigationManager;
        readonly private INavigationService _navigationService;
        public SportsManagementModule
            (IUnityContainer container, IRegionManager regionManager,
            IWidgetManager widgetManager,INavigationManager navigationManager,
            INavigationService navigationService)
        {
            _regionManager = regionManager;
            _container = container;
            _widgetManager = widgetManager;
            _navigationManager = navigationManager;
            _navigationService = navigationService;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterDependencies();

            ImportData();
            
            RegisterWidgets();

            RegisterNavigations();
        }

        #endregion

        private void RegisterNavigations()
        {
            _navigationManager.AddRegionIfMissing(ModuleRegionNames.SportManagementRegion, "运动");
            _navigationManager.Regions[ModuleRegionNames.SportManagementRegion].Add(
                new NavigationDescriptor()
                {
                    Header = "运动计划",
                    ViewName = "SportsPlan",
                    ViewResolver = () => _container.Resolve<CalendarView>()
                });
            _navigationManager.Regions[ModuleRegionNames.SportManagementRegion].Add(
                new NavigationDescriptor()
                {
                    Header = "运动面板",
                    ViewName = "SportsPanel",
                    ViewResolver = () => _container.Resolve<ModuleShell>()
                });
        }




        private void RegisterWidgets()
        {
            
            IWidgetManager manager = _container.Resolve<IWidgetManager>();
            manager.Add(typeof(DailySportWidget));
        }

        private void ImportData()
        {
            ConfigImporters();

            var manager = _container.Resolve<IDataImportManager>();
            manager.ImportData();
        }

        private void ConfigImporters()
        {
            IImporterCollection collection = _container.Resolve<IImporterCollection>();
            collection.Add(
                new CategoryImporter 
                    (new XmlCategorySource(@"data/sport/SportData.xml"),
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
                .RegisterType<IDataImportManager, DataImportManager>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IImporterCollection>(new ImporterCollection())

                //Services
                .RegisterType(typeof(IProvider<>), typeof(ProviderBase<>))
                .RegisterType<ICategoryProvider, CategoryProvider>()
                .RegisterType<ISportProvider, SportProvider>()
                .RegisterType<ISportsPlanProvider, SportsPlanProvider>()
                .RegisterType<IPlanItemProvider, PlanItemProvider>()
                .RegisterInstance<ISportsPlanItemViewModelFactory>(new SportsPlanItemViewModelFactory())
                .RegisterInstance<ISportCardViewModelFactory>(new SportCardViewModelFactory())
                .RegisterType<ISportsPlanViewModelFactory, SportsPlanViewModelFactory>()
                .RegisterType<ISportsPlanViewModelFactory, SportsPlanViewModelFactory>()

                //ViewModels
                .RegisterType<ICategoriesPanelViewModel, CategoriesPanelViewModel>()
                .RegisterType<ISportsPanelViewModel, SportsPanelViewModel>()
                .RegisterType<ISportsPlanViewModel, SportsPlanViewModel>()
                .RegisterType<IPlanListViewModel, PlanListViewModel>()
                .RegisterType<ICalendarViewModel,CalendarViewModel>()

                //Views
                .RegisterType<ISportsPanelView, SportsPanelView>()
                .RegisterType<ICategoriesPanelView, CategoriesPanelView>()
                .RegisterType<ISportsPlanView, SportsPlanView>()
                .RegisterType<IPlanListView, PlanListView>()

                .RegisterType<CalendarView>(new ContainerControlledLifetimeManager())

                .RegisterType<ModuleShell>(new ContainerControlledLifetimeManager());
        }

        #endregion
    
    }
}
