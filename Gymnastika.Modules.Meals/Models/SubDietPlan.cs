using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class SubDietPlan
    {
        public virtual int Id { get; set; }

        public virtual DietPlan DietPlan { get; set; }

        public virtual IList<DietPlanItem> DietPlanItems { get; set; }
    }
}
