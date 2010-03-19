using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Sync.Communication
{
    public class NetworkAdapterCollection : List<NetworkAdapter>
    {
        public NetworkAdapterCollection() { }
        public NetworkAdapterCollection(IEnumerable<NetworkAdapter> adapters) : base(adapters) { }
    }
}
