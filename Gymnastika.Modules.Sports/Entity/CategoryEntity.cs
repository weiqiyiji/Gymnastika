using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Entity
{
    public class CategoryEntity
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ImageUri { set; get; }

        public virtual string Note { get; set; }

    }
}
