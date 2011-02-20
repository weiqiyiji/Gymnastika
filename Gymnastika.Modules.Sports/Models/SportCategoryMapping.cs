using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportCategoryMapping
    {
        public virtual int Id { get; set; }

        public virtual int SportId { get; set; }

        public virtual int CategoryId { get; set; }
    }
}
