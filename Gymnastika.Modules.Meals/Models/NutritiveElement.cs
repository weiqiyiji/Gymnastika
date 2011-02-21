using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Meals.Models
{
    public class NutritiveElement
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Unit { get; set; }

        public virtual int Content { get; set; }

        public virtual Food Food { get; set; }
    }
}
