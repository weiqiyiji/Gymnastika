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
using Gymnastika.Modules.Meals.Communication.Services;

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
            RegisterController();
            RegisterNavigation();
        }

        #endregion

        private void RegisterWidgets()
        {
            _widgetMananger.Add(typeof(BMIWidget));
            _widgetMananger.Add(typeof(TodayDietPlanWidget));
            _widgetMananger.Add(typeof(OneKeyScoreWidget));
        }

        private void RegisterServices()
        {
            _container.RegisterType<IFoodService, FoodService>(new ContainerControlledLifetimeManager())
                .RegisterType<ICategoryProvider, CategoryProvider>()
                .RegisterType<IFoodProvider, FoodProvider>()
                .RegisterType<INutritionElementProvider, NutritionElementProvider>()
                .RegisterType<IFavoriteFoodProvider, FavoriteFoodProvider>()
                .RegisterType<IDietPlanProvider, DietPlanProvider>()
                .RegisterType<ISubDietPlanProvider, SubDietPlanProvider>()
                .RegisterType<IDietPlanItemProvider, DietPlanItemProvider>()
                .RegisterType<IDietPlanTaskProvider, DietPlanTaskProvider>()
                .RegisterInstance(typeof(CommunicationService), new ContainerControlledLifetimeManager());
        }

        private void RegisterViews()
        {
            _container.RegisterType<IFoodListView, FoodListView>()
                .RegisterType<IDietPlanListView, DietPlanListView>()
                .RegisterType<IDietPlanSubListView, DietPlanSubListView>()
                .RegisterType<IFoodDetailView, FoodDetailView>()
                .RegisterType<IMealsManagementView, MealsManagementView>(new ContainerControlledLifetimeManager())
                .RegisterType<ISelectDietPlanView, SelectDietPlanView>()
                .RegisterType<IBMIIntroductionView, BMIIntroductionView>()
                .RegisterType<ICategoryListView, CategoryListView>()
                .RegisterType<INutritionChartView, NutritionChartView>()
                .RegisterType<IDietPlanNutritionChartView, DietPlanNutritionChartView>()
                .RegisterType<IPositionedFoodView, PositionedFoodView>()
                .RegisterType<IRecommendedDietPlanView, RecommendedDietPlanView>(new ContainerControlledLifetimeManager())
                .RegisterType<IHistoryDietPlanView, HistoryDietPlanView>(new ContainerControlledLifetimeManager())
                .RegisterType<ITodayDietPlanView, TodayDietPlanView>();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IFoodListViewModel, FoodListViewModel>()
                .RegisterType<IDietPlanListViewModel, DietPlanListViewModel>()
                .RegisterType<IDietPlanSubListViewModel, DietPlanSubListViewModel>()
                .RegisterType<IFoodDetailViewModel, FoodDetailViewModel>()
                .RegisterType<IMealsManagementViewModel, MealsManagementViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ISelectDietPlanViewModel, SelectDietPlanViewModel>()
                .RegisterType<ICategoryListViewModel, CategoryListViewModel>()
                .RegisterType<INutritionChartViewModel, NutritionChartViewModel>()
                .RegisterType<IDietPlanNutritionChartViewModel, DietPlanNutritionChartViewModel>()
                .RegisterType<IPositionedFoodViewModel, PositionedFoodViewModel>()
                .RegisterType<IRecommendedDietPlanViewModel, RecommendedDietPlanViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<IHistoryDietPlanViewModel, HistoryDietPlanViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ITodayDietPlanViewModel, TodayDietPlanViewModel>();
        }

        private void RegisterNavigation()
        {
            _navigationManager.AddRegionIfMissing(NavigationNames.ShellRegion, "健康饮食");
            _navigationManager.Regions[NavigationNames.ShellRegion].Add(
                new NavigationDescriptor()
                {
                    Header = "创建饮食计划",
                    ViewName = NavigationNames.CreateDietPlanView,
                    ViewResolver = () => (MealsManagementView)_container.Resolve<IMealsManagementViewModel>().View,
                });

            _navigationManager.Regions[NavigationNames.ShellRegion].Add(
                new NavigationDescriptor()
                {
                    Header = "选择推荐计划",
                    ViewName = NavigationNames.RecommendedDietPlanView,
                    ViewResolver = () => (RecommendedDietPlanView)_container.Resolve<IRecommendedDietPlanViewModel>().View,
                });

            _navigationManager.Regions[NavigationNames.ShellRegion].Add(
                new NavigationDescriptor()
                {
                    Header = "饮食计划查询",
                    ViewName = NavigationNames.HistoryDietPlanView,
                    ViewResolver = () => (HistoryDietPlanView)_container.Resolve<IHistoryDietPlanViewModel>().View,
                });
        }

        private void RegisterController()
        {
            _container.RegisterType(typeof(SelectDateView));
            _container.RegisterType<ILoadDataController, LoadDataController>();
            var loadDataController = _container.Resolve<ILoadDataController>();
            if (!loadDataController.IsLoaded)
                _container.Resolve<ILoadDataController>().Load();
        }
    }
}
