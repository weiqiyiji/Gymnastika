using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gymnastika.Data.Configuration
{
    public class AutomappingConfigurationElement : ConfigurationElement
    {
        public AutomappingConfigurationElement() { }

        public AutomappingConfigurationElement(string assemblyName)
        {
            base["name"] = assemblyName;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string AssemblyName
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
    }
}
