using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class DietPlanTask
    {
        public virtual int Id { get; set; }

        public virtual int Year { get; set; }

        public virtual int Month { get; set; }

        public virtual int Day { get; set; }

        public virtual int SubDietPlanId { get; set; }

        public virtual int TaskId { get; set; }
    }
}
