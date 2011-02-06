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
            RegisterViews();
            RegisterViewModels();
            RegisterViewWithRegion();
        }

        #endregion

        private void RegisterViews()
        {
            _container.RegisterType<IFoodListView, FoodListView>()
                .RegisterType<IDietPlanListView, DietPlanListView>()
                .RegisterType<IFoodDetailView, FoodDetailView>()
                .RegisterType<IMealsManagementView, MealsManagementView>(new ContainerControlledLifetimeManager())
                .RegisterType<ICreateDietPlanView, CreateDietPlanView>()
                .RegisterType<IRecommendDietPlanView, RecommendDietPlanView>();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IFoodListViewModel, FoodListViewModel>()
                .RegisterType<IDietPlanListViewModel, DietPlanListViewModel>()
                .RegisterType<IFoodDetailViewModel, FoodDetailViewModel>()
                .RegisterType<IMealsManagementViewModel, MealsManagementViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ICreateDietPlanViewModel, CreateDietPlanViewModel>()
                .RegisterType<IRecommendDietPlanViewModel, RecommendDietPlanViewModel>();
        }

        private void RegisterViewWithRegion()
        {
            IMealsManagementViewModel mealsManagementViewModel = _container.Resolve<IMealsManagementViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => mealsManagementViewModel.View);
        }
    }
}
