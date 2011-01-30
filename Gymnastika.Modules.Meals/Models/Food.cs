using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class Food
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SmallImageUri { get; set; }

        public string LargeImageUri { get; set; }

        public int Calorie { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public IEnumerable<NutritiveElement> NutritionalContent { get; set; }

        public string Introduction { get; set; }

        public string NutritionalValue { get; set; }

        public string Function { get; set; }

        public string SuitableCrowd { get; set; }

        public IEnumerable<Food> RelatedFoods { get; set; }
    }
}
