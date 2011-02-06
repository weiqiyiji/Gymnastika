using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Providers
{
    public interface IDataServicesProviderFactory
    {
        IDataServicesProvider CreateProvider(DataServiceParameters parameters);
    }
}
