using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class NutritionalElement
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int Value { get; set; }

        public virtual Food Food { get; set; }
    }
}
