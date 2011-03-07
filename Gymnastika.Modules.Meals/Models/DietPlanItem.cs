using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class DietPlanItem
    {
        public virtual int Id { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual Food Food { get; set; }

        public virtual SubDietPlan SubDietPlan { get; set; }
    }
}
