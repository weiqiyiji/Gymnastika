using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace Gymnastika.Data.Configuration
{
    public class FileAutomappingConfigurer : IAutomappingConfigurer
    {
        #region IAutomappingConfiguration Members

        public IEnumerable<AutomappingConfigurationMetadata> GetAutomappingAssemblies()
        {
            AutomappingConfigurationSection AutomappingSection =
                ConfigurationManager.GetSection("auto-mappings") as AutomappingConfigurationSection;

            IList<AutomappingConfigurationMetadata> metadataCollection = new List<AutomappingConfigurationMetadata>();

            foreach (AutomappingConfigurationElement Automapping in AutomappingSection.Mappings)
            { 
                AutomappingConfigurationMetadata metadata = new AutomappingConfigurationMetadata();
                metadata.AssemblyName = Automapping.AssemblyName;

                metadataCollection.Add(metadata);
            }

            return metadataCollection;
        }

        #endregion
    }
}