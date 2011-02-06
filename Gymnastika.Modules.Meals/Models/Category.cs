using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUri { get; set; }

        public IEnumerable<SubCategory> SubCategory { get; set; }
    }
}
