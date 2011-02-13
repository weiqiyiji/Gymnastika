using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Configuration;

namespace Gymnastika.Data.Providers
{
    public class SqlCeDataServicesProviderFactory : IDataServicesProviderFactory
    {
        private IAutomappingConfigurer _configurer;

        public SqlCeDataServicesProviderFactory(IAutomappingConfigurer configurer) 
        {
            _configurer = configurer;
        }

        public IDataServicesProvider CreateProvider(DataServiceParameters parameters)
        {
            return new SqlCeDataServicesProvider(parameters.DataFolder, parameters.DatabaseName)
            {
                AutomappingConfigurer = _configurer
            };
        }
    }
}
