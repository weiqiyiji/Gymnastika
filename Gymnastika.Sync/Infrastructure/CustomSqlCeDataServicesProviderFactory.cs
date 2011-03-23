using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Data.Providers;
using Gymnastika.Data.Configuration;

namespace Gymnastika.Sync.Infrastructure
{
    public class CustomSqlCeDataServicesProviderFactory : IDataServicesProviderFactory
    {
        private IAutomappingConfigurer _configurer;

        public CustomSqlCeDataServicesProviderFactory(IAutomappingConfigurer configurer) 
        {
            _configurer = configurer;
        }

        public IDataServicesProvider CreateProvider(DataServiceParameters parameters)
        {
            return new CustomSqlCeDataServicesProvider(parameters.DataFolder, parameters.DatabaseName)
            {
                AutomappingConfigurer = _configurer
            };
        }
    }
}