using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class UserDietPlanMapping
    {
        public virtual int Id { get; set; }

        public virtual IList<DietPlan> DietPlans { get; set; }
    }
}
