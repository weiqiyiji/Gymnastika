using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class DietPlan
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual PlanType PlanType { get; set; }

        public virtual IList<SubDietPlan> SubDietPlans { get; set; }
    }

    public enum PlanType
    {
        CreatedDietPlan,
        RecommendedDietPlan
    }
}
