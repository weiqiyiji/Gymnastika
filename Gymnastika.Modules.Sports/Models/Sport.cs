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

        public virtual string Introduction { get; set; }

        public virtual int CaloriePerHour { get; set; }

        public virtual IList<SportsCategory> SportsCategories { get; set; }
    }
}
