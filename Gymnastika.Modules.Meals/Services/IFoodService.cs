using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Services.Providers;

namespace Gymnastika.Modules.Meals.Services
{
    public interface IFoodService
    {
        ICategoryProvider CategoryProvider { get; set; }
        ISubCategoryProvider SubCategoryProvider { get; set; }
        IFoodProvider FoodProvider { get; set; }
        IIntroductionProvider IntroductionProvider { get; set; }
        INutritionalElementProvider NutritionalElementProvider { get; set; }
        IDietPlanProvider DietPlanProvider { get; set; }
        ISubDietPlanProvider SubDietPlanProvider { get; set; }
        IDietPlanItemProvider DietPlanItemProvider { get; set; }
        IFavoriteFoodProvider FavoriteFoodProvider { get; set; }
    }
}
