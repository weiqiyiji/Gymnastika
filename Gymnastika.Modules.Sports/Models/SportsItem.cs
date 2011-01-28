using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsItem
    {
        public string Name { set; get; }
        public DateTime Time { set; get; }
        public int Duration { get; set; }   //Min
        public string PictureUri { get; set; }
    }
}
