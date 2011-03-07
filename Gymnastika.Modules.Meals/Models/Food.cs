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

        public virtual string SmallImageUri { get; set; }

        public virtual string MiddleImageUri { get; set; }

        public virtual string LargeImageUri { get; set; }

        public virtual decimal Calorie { get; set; }

        public virtual IList<NutritionalElement> NutritionalElements { get; set; }

        public virtual IList<Introduction> Introductions { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        //public virtual IList<DietPlanItem> DietPlanItems { get; set; }

        //public virtual IList<FavoriteFood> FavoriteFoods { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
