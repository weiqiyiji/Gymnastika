using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Gymnastika.Common.Configuration
{
    public class ShellSettings
    {
        public string Name { get; set; }
        public string DataProvider { get; set; }
        public string DataConnectionString { get; set; }
        public string DataTablePrefix { get; set; }
    }
}
