using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Entity
{
    public class SportEntity
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string SmallImageUri { set; get; }

        public virtual string LargeImageUri { get; set; }

        public virtual string BriefIntroduction { get; set; }

        public virtual string DetailIntroduction { get; set; }

        public virtual string CaloriePerHour { get; set; }
    }
}
