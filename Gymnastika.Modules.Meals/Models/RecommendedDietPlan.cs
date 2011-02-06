using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class RecommendedDietPlan
    {
        public string Id { get; set; }

        public IList<Food> Foods { get; set; }
    }
}
