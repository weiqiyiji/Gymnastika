using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Providers
{
    public class DataServicesProviderInitializationException : Exception
    {
        public DataServicesProviderInitializationException(IDataServicesProvider provider, string message)
            : base(string.Format("{0} -- {1}", provider.GetType(), message))
        {}
    }
}
