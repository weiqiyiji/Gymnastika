using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class Category
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ImageUri { get; set; }

        public virtual IList<SubCategory> SubCategories { get; set; }
    }
}
