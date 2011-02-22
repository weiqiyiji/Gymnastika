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

        public virtual string LargeImageUri { get; set; }

        public virtual int Calorie { get; set; }

        public virtual IList<NutritiveElement> NutritionalContent { get; set; }

        public virtual string Introduction { get; set; }

        public virtual string NutritionalValue { get; set; }

        public virtual string Function { get; set; }

        public virtual string SuitableCrowd { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual IList<SubDietPlan> SubDietPlans { get; set; }
    }
}
