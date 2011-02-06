using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Providers
{
    public class DataServiceParameters
    {
        public bool CreateDatabase { get; set; }
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
    }
}
