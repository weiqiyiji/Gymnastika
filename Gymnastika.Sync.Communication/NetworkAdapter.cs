using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Sync.Communication
{
    public class NetworkAdapter
    {
        public string IpAddress { get; set; }
        public string SubnetMask { get; set; }
        public string DefaultGateway { get; set; }
    }
}
