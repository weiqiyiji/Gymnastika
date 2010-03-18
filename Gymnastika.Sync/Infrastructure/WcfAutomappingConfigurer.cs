using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Data.Configuration;
using System.Configuration;

namespace Gymnastika.Sync.Infrastructure
{
    public class WcfAutomappingConfigurer : IAutomappingConfigurer
    {
        public IEnumerable<AutomappingConfigurationMetadata> GetAutomappingMetadata()
        {
            AutomappingConfigurationSection AutomappingSection =
                ConfigurationManager.GetSection("auto-mappings") as AutomappingConfigurationSection;

            IList<AutomappingConfigurationMetadata> metadataCollection = new List<AutomappingConfigurationMetadata>();

            foreach (AutomappingConfigurationElement Automapping in AutomappingSection.Mappings)
            {
                AutomappingConfigurationMetadata metadata = new AutomappingConfigurationMetadata();
                metadata.AssemblyName = HttpContext.Current.Server.MapPath("~/bin/" + Automapping.AssemblyName);

                metadataCollection.Add(metadata);
            }

            return metadataCollection;
        }
    }
}