using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class Connection
    {
        public virtual int Id { get; set; }
        public virtual DesktopClient Source { get; set; }
        public virtual PhoneClient Target { get; set; }
    }
}