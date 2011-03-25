using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface IPositionedFoodViewModel
    {
        IPositionedFoodView View { get; set; }
        string ImageUri { get; }
        string FoodName { get; }
        string Calorie { get; }
    }
}
