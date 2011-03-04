using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Services.Models;

namespace Gymnastika.Modules.Meals.Models
{
    public class FavoriteFood
    {
        public virtual int Id { get; set; }

        public virtual User User { get; set; }

        public virtual IList<Food> Foods { get; set; }
    }
}
