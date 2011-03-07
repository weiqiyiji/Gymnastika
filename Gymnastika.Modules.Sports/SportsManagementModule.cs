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
            //ImportData();
            //RegisterRegions();
        }

        private void ImportData()
        {
            IDataImporter<SportsCategory> importer = _container.Resolve<IDataImporter<SportsCategory>>();
            
            if (importer.NeedImport())
            {
                XmlCatagoryProvider provider = new XmlCatagoryProvider();
                importer.ImportData(provider.Fetch(t=>true));
            }
        }


        //private void RegisterRegions()
        //{
        //    _regionManager
        //        .RegisterViewWithRegion(RegionNames.DisplayRegion, typeof(Shell))
        //        .RegisterViewWithRegion(SportRegionNames.SportRegion, typeof(ISportsPanelView))
        //        .RegisterViewWithRegion(SportRegionNames.CategoryRegion, typeof(ICategoriesPanelView))
        //        .RegisterViewWithRegion(SportRegionNames.SportPlan,typeof(ISportsPlanView));
        //}

        #endregion


        #region RegisterDependencies

        private void RegisterDependencies()
        {
            //Mock
            _container
             //Services
                .RegisterType<ICategoriesProvider, XmlCatagoryProvider>(new ContainerControlledLifetimeManager())
             
             //Shell
             .RegisterInstance(new Shell());


            //Dependency
            _container

                //Services
                .RegisterType<IDataImporter<SportsCategory>,CategoryDataImporter>(new ContainerControlledLifetimeManager())
                //.RegisterType<ICategoriesProvider, CategoriesProvider>(new ContainerControlledLifetimeManager())

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
