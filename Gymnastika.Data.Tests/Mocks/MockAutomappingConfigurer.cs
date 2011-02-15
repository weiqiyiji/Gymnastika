using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Configuration;
using System.Reflection;

namespace Gymnastika.Data.Tests.Mocks
{
    public class MockAutomappingConfigurer : IAutomappingConfigurer
    {
        private const string DataAssemblyFile = "Gymnastika.Data.dll";
        private const string DataTestsAssemblyFile = "Gymnastika.Data.Tests.dll";

        #region IAutomappingConfigurer Members

        public IEnumerable<AutomappingConfigurationMetadata> GetAutomappingMetadata()
        {
            yield return new AutomappingConfigurationMetadata { AssemblyName = MockAutomappingConfigurer.DataAssemblyFile };
            yield return new AutomappingConfigurationMetadata { AssemblyName = MockAutomappingConfigurer.DataTestsAssemblyFile };
        }

        #endregion

        public int ModelCount
        {
            get
            {
                return Assembly.LoadFrom(DataAssemblyFile)
                               .GetExportedTypes()
                               .Count(t => t.Namespace.Contains(".Models")) +
                       Assembly.LoadFrom(DataTestsAssemblyFile)
                               .GetExportedTypes()
                               .Count(t => t.Namespace.Contains(".Models"));
            }
        }
    }
}
