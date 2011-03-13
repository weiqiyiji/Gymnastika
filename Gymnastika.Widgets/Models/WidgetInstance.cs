using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Services.Models;

namespace Gymnastika.Widgets.Models
{
    public class WidgetInstance
    {
        public virtual int Id { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Icon { get; set; }
        public virtual string Type { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual User User { get; set; }
    }
}
