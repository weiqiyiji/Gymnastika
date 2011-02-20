using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Services;
using System.ComponentModel.Composition;

namespace Gymnastika.Modules.Sports.Models
{

    public class SportsCategory 
    {
        public virtual int Id { set; get; }

        public virtual string Name { get; set; }

        public virtual string ImageUri { set; get; }

        public virtual string Note { get; set; }

        public virtual IEnumerable<Sport> Sports { get; set; }

    }
}
