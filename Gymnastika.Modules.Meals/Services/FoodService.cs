using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Services.Models;
using Gymnastika.Modules.Meals.Services.Providers;

namespace Gymnastika.Modules.Meals.Services
{
    public class FoodService : IFoodService
    {
        //private readonly IWorkEnvironment _workEnvironment;
        //private readonly IRepository<Category> _categoryRepository;
        //private readonly IRepository<SubCategory> _subCategoryRepository;
        //private readonly IRepository<Food> _foodRepository;
        //private readonly IRepository<DietPlan> _dietPlanRepository;
        //private readonly IRepository<SubDietPlan> _subDietPlanRepository;
        //private readonly IRepository<DietPlanItem> _dietPlanItemRepository;
        ////private readonly IRepository<UserDietPlanMapping> _userDietPlanMappingRepository;
        //private readonly IRepository<User> _userRepository;

        //public FoodService(IWorkEnvironment workEnvironment,
        //    IRepository<Category> categoryRepository,
        //    IRepository<SubCategory> subCategoryRepository,
        //    IRepository<Food> foodRepository,
        //    IRepository<DietPlan> dietPlanRepository,
        //    IRepository<SubDietPlan> subDietPlanRepository,
        //    IRepository<DietPlanItem> dietPlanItemRepository,
        //    //IRepository<UserDietPlanMapping> userDietPlanMappingRepository,
        //    IRepository<User> userRepository)
        //{
        //    _workEnvironment = workEnvironment;
        //    _categoryRepository = categoryRepository;
        //    _subCategoryRepository = subCategoryRepository;
        //    _foodRepository = foodRepository;
        //    _dietPlanRepository = dietPlanRepository;
        //    _subDietPlanRepository = subDietPlanRepository;
        //    _dietPlanItemRepository = dietPlanItemRepository;
        //    //_userDietPlanMappingRepository = userDietPlanMappingRepository;
        //    _userRepository = userRepository;
        //}

        public FoodService(ICategoryProvider categoryProvider,
            IFoodProvider foodProvider,
            INutritionElementProvider nutritionalElementProvider,
            IDietPlanProvider dietPlanProvider,
            ISubDietPlanProvider subDietPlanProvider,
            IDietPlanItemProvider dietPlanItemProvider,
            IFavoriteFoodProvider favoriteFoodProvider)
        {
            CategoryProvider = categoryProvider;
            FoodProvider = foodProvider;
            NutritionElementProvider = nutritionalElementProvider;
            DietPlanProvider = dietPlanProvider;
            SubDietPlanProvider = subDietPlanProvider;
            DietPlanItemProvider = dietPlanItemProvider;
            FavoriteFoodProvider = favoriteFoodProvider;
        }

        #region IFoodService Members

        public ICategoryProvider CategoryProvider { get; set; }

        public IFoodProvider FoodProvider { get; set; }

        public INutritionElementProvider NutritionElementProvider { get; set; }

        public IDietPlanProvider DietPlanProvider { get; set; }

        public ISubDietPlanProvider SubDietPlanProvider { get; set; }

        public IDietPlanItemProvider DietPlanItemProvider { get; set; }

        public IFavoriteFoodProvider FavoriteFoodProvider { get; set; }

        #endregion
    }
}
