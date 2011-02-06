using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gymnastika.Data.Configuration
{
    public class AutomappingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false)]
        public AutomappingConfigurationElementCollection Mappings
        {
            get { return (AutomappingConfigurationElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }
}
