using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class Food
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ImageUri { get; set; }

        public virtual decimal Calorie { get; set; }

        public virtual IList<NutritionElement> NutritionElements { get; set; }

        public virtual Category Category { get; set; }

        public virtual IList<DietPlanItem> DietPlanItems { get; set; }

        public virtual IList<FavoriteFood> FavoriteFoods { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
