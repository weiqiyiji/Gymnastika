using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Events
{
    public class DietPlanNutritionChangedEvent : CompositePresentationEvent<IList<NutritionElement>>
    {
    }
}
