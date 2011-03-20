using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class DesktopClient
    {
        public virtual int Id { get; set; }
        public virtual string Placeholder { get; set; }
        public virtual IList<NetworkAdapter> NetworkAdapters { get; set; }
    }
}