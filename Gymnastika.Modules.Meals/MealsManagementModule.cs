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

namespace Gymnastika.Modules.Meals
{
    public class MealsManagementModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public MealsManagementModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterServices();
            RegisterViews();
            RegisterViewModels();
            RegisterViewWithRegion();
            StoreDataToDatabase();
        }

        #endregion

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
                .RegisterType<IMealsManagementView, MealsManagementView>(new ContainerControlledLifetimeManager())
                .RegisterType<ICreateDietPlanView, CreateDietPlanView>()
                .RegisterType<ISelectDietPlanView, SelectDietPlanView>();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IFoodListViewModel, FoodListViewModel>()
                .RegisterType<IDietPlanListViewModel, DietPlanListViewModel>()
                .RegisterType<IDietPlanSubListViewModel, DietPlanSubListViewModel>()
                .RegisterType<IFoodDetailViewModel, FoodDetailViewModel>()
                .RegisterType<IMealsManagementViewModel, MealsManagementViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ICreateDietPlanViewModel, CreateDietPlanViewModel>()
                .RegisterType<ISelectDietPlanViewModel, SelectDietPlanViewModel>();
        }

        private void RegisterViewWithRegion()
        {
            IRegion displayRegion = _regionManager.Regions[RegionNames.DisplayRegion];

            IMealsManagementViewModel mealsManagementViewModel = _container.Resolve<IMealsManagementViewModel>();
            displayRegion.Add(mealsManagementViewModel.View);
            displayRegion.Activate(mealsManagementViewModel.View);
        }

        private void StoreDataToDatabase()
        {
            XDataHelpers.XDataRepository dataSource = new XDataHelpers.XDataRepository(_container.Resolve<IFoodService>());
            dataSource.BeginStore();
        }
    }
}
