using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Providers
{
    public class SqlCeDataServicesProviderFactory : IDataServicesProviderFactory
    {
        public SqlCeDataServicesProviderFactory() { }

        public IDataServicesProvider CreateProvider(DataServiceParameters parameters)
        {
            return new SqlCeDataServicesProvider("~/Data", parameters.ConnectionString);
        }
    }
}
