using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Configuration;

namespace Gymnastika.Data.Tests.Mocks
{
    public class MockAutomappingConfigurer : IAutomappingConfigurer
    {
        #region IAutomappingConfigurer Members

        public IEnumerable<AutomappingConfigurationMetadata> GetAutomappingMetadata()
        {
            yield return new AutomappingConfigurationMetadata { AssemblyName = "Gymnastika.Data.Tests.dll" };
            yield return new AutomappingConfigurationMetadata { AssemblyName = "Gymnastika.Data.dll" };
        }

        #endregion
    }
}
