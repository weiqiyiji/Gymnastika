using System.Collections.Generic;

namespace Gymnastika.Data.Configuration
{
    public interface IAutomappingConfigurer
    {
        IEnumerable<AutomappingConfigurationMetadata> GetAutomappingAssemblies();
    }
}
