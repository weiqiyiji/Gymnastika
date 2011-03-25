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
            //return;
            RegisterDependencies();

            ImportData();
            
            RegisterWidgets();

            RegisterNavigations();
        }

        #endregion

        private void RegisterNavigations()
        {
            const string RegionName = "SportManagementRegion";
            _navigationManager.AddRegionIfMissing(RegionName, "运动");

            INavigationRegion region = _navigationManager.Regions[RegionName];
            
            //历史计划
            region.Add(
                new NavigationDescriptor()
                {
                    Header = "本周计划",
                    ViewName = NavigationNames.PlanPanel,
                    ViewResolver = () => _container.Resolve<CompostieChartView>(),
                });

            //创建计划
            region.Add(
                new NavigationDescriptor()
                {
                    Header = "定制运动计划",
                    ViewName = NavigationNames.CreatePlanPanel,
                    ViewResolver = () => _container.Resolve<CompositePanel>(),
                    //States = new List<ViewState>()
                    //{
                    //    new ViewState()
                    //    {
                    //        Header = "制定计划",
                    //        Name = "CreatePlan",
                    //    },
                    //    new ViewState()
                    //    { 
                    //        Header = "查看运动详情",
                    //        Name = "SportDetail",
                    //    },

                    //},
                    //StateChanging = _container.Resolve<CompositePanel>().StateChanging,
                });
            //图表
            region.Add(
                new NavigationDescriptor()
                {
                    Header = "运动情况图表",
                    ViewName = NavigationNames.ChartPanel,
                    ViewResolver = () => _container.Resolve<ChartView>(),
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
                .RegisterType<ISportViewModel, SportViewModel>()
                .RegisterType<ICompositePanelViewModel, CompositePanelViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISportCalorieChartViewModel, SportCalorieChartViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType < IPlanDetailViewModel,PlanDetailViewModel>(new ContainerControlledLifetimeManager())
                //Views
                .RegisterType<ISportsPanelView, SportsPanelView>()
                .RegisterType<ICategoriesPanelView, CategoriesPanelView>()
                .RegisterType<ISportsPlanView, SportsPlanView>()
                .RegisterType<CompostieChartView>(new ContainerControlledLifetimeManager())
                .RegisterType<PlanListView>(new ContainerControlledLifetimeManager())
                .RegisterType<CompositePanel>(new ContainerControlledLifetimeManager())
                .RegisterType<ChartView>(new ContainerControlledLifetimeManager());
       }

        #endregion
    
    }
}
