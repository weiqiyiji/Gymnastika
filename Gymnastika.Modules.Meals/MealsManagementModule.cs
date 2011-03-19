using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Meals.ViewModels;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Modules.Meals.Services.Providers;
using Gymnastika.Widgets;
using Gymnastika.Modules.Meals.Widgets;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Controllers;
using Gymnastika.Common.Navigation;

namespace Gymnastika.Modules.Meals
{
    public class MealsManagementModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IWidgetManager _widgetMananger;
        private readonly INavigationManager _navigationManager;

        public MealsManagementModule(IUnityContainer container,
            IRegionManager regionManager, 
            IWidgetManager widgetManager,
            INavigationManager navigationManager)
        {
            _container = container;
            _regionManager = regionManager;
            _widgetMananger = widgetManager;
            _navigationManager = navigationManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterWidgets();
            RegisterServices();
            RegisterViews();
            RegisterViewModels();
            RegisterViewWithRegion();
            StoreDataToDatabase();
        }

        #endregion

        private void RegisterWidgets()
        {
            _widgetMananger.Add(typeof(BMIWidget));
        }

        private void RegisterServices()
        {
            _container.RegisterType<IFoodService, FoodService>()
                .RegisterType<ICategoryProvider, CategoryProvider>()
                .RegisterType<ISubCategoryProvider, SubCategoryProvider>()
                .RegisterType<IFoodProvider, FoodProvider>()
                .RegisterType<IIntroductionProvider, IntroductionProvider>()
                .RegisterType<INutritionalElementProvider, NutritionalElementProvider>()
                .RegisterType<IFavoriteFoodProvider, FavoriteFoodProvider>()
                .RegisterType<IDietPlanProvider, DietPlanProvider>()
                .RegisterType<ISubDietPlanProvider, SubDietPlanProvider>()
                .RegisterType<IDietPlanItemProvider, DietPlanItemProvider>();
        }

        private void RegisterViews()
        {
            _container.RegisterType<IFoodListView, FoodListView>()
                .RegisterType<IDietPlanListView, DietPlanListView>()
                .RegisterType<IDietPlanSubListView, DietPlanSubListView>()
                .RegisterType<IFoodDetailView, FoodDetailView>()
                .RegisterType<IMealsManagementView, MealsManagementView>()
                .RegisterType<ICreateDietPlanView, CreateDietPlanView>()
                .RegisterType<ISelectDietPlanView, SelectDietPlanView>()
                .RegisterType<IBMIIntroductionView, BMIIntroductionView>()
                .RegisterType<ICategoryListView, CategoryListView>()
                .RegisterType<ICategoryItemView, CategoryItemView>()
                .RegisterType<INutritionChartView, NutritionChartView>()
                .RegisterType<INutritionChartItemView, NutritionChartItemView>();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IFoodListViewModel, FoodListViewModel>()
                .RegisterType<IDietPlanListViewModel, DietPlanListViewModel>()
                .RegisterType<IDietPlanSubListViewModel, DietPlanSubListViewModel>()
                .RegisterType<IFoodDetailViewModel, FoodDetailViewModel>()
                .RegisterType<IMealsManagementViewModel, MealsManagementViewModel>()
                .RegisterType<ICreateDietPlanViewModel, CreateDietPlanViewModel>()
                .RegisterType<ISelectDietPlanViewModel, SelectDietPlanViewModel>()
                .RegisterType<ICategoryListViewModel, CategoryListViewModel>()
                .RegisterType<ICategoryItemViewModel, CategoryItemViewModel>()
                .RegisterType<INutritionChartViewModel, NutritionChartViewModel>()
                .RegisterType<INutritionChartItemViewModel, NutritionChartItemViewModel>();
        }

        private void RegisterViewWithRegion()
        {
            _container.RegisterType<ILoadDataController, LoadDataController>()
                .RegisterType(typeof(LoadDataView));
            //IRegion displayRegion = _regionManager.Regions[RegionNames.DisplayRegion];

            //IMealsManagementViewModel mealsManagementViewModel = _container.Resolve<IMealsManagementViewModel>();
            //displayRegion.Add(mealsManagementViewModel.View);
            //displayRegion.Activate(mealsManagementViewModel.View);

            //IMealsManagementViewModel mealsManagementViewModel = _container.Resolve<IMealsManagementViewModel>();
            //_regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => mealsManagementViewModel.View);

            //_navigationManager.AddIfMissing(
            //    new NavigationDescriptor()
            //    {
            //        ViewType = typeof(MealsManagementView),
            //        ViewName = "MealsManagementView",
            //        Label = "饮 食",
            //        RegionName = RegionNames.MainRegion
            //    }, true);

        }

        private delegate void ThreadDelegate();
        private void StoreDataToDatabase()
        {
            //XDataHelpers.XDataRepository dataSource = new XDataHelpers.XDataRepository(_container.Resolve<IFoodService>(), _container.Resolve<IWorkEnvironment>());
            //ThreadDelegate backGroundLoader = new ThreadDelegate(dataSource.Store);
            //backGroundLoader.BeginInvoke(null, null);
            //System.Threading.Thread.Sleep(1 * 60 * 10000);
            //if (!dataSource.IsStored)
                //dataSource.Store();

            //ILoadDataController loadDataController = _container.Resolve<ILoadDataController>();
            //if (!loadDataController.IsLoaded)
            //    loadDataController.Load();
        }
    }
}
