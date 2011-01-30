using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Meals.ViewModels;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals
{
    public class MealsManagementModule : IModule
    {
        private readonly IUnityContainer _container;

        public MealsManagementModule(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViews();
            RegisterViewModels();
        }

        #endregion

        private void RegisterViews()
        {
            _container.RegisterType<IFoodListView, FoodListView>()
                .RegisterType<IDietPlanListView, DietPlanListView>()
                .RegisterType<IFoodDetailView, FoodDetailView>();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IFoodListViewModel, FoodListViewModel>()
                .RegisterType<IDietPlanListViewModel, DietPlanListViewModel>()
                .RegisterType<IFoodDetailViewModel, FoodDetailViewModel>();
        }
    }
}
