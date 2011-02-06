using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class SubCategory
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Food> Foods { get; set; }
    }
}
