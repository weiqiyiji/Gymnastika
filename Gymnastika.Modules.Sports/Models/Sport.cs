using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Media;

namespace Gymnastika.Modules.Sports.Models
{
    public class Sport
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ImageUri { get; set; }

        public virtual string Brief { get; set; }

        public virtual string IntroductionUri { get; set; }

        public virtual double Calories { get; set; }

        public virtual int Minutes { get; set; }

        public virtual IList<SportsCategory> SportsCategories { get; set; }
    }
}
