using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class NetworkAdapter
    {
        public virtual int Id { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual string SubnetMask { get; set; }
        public virtual string DefaultGateway { get; set; }
        public virtual DesktopClient Client { get; set; }
    }
}