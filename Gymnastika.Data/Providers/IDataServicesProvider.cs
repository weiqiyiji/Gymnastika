using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;

namespace Gymnastika.Data.Providers
{
    public interface IDataServicesProvider
    {
        NHibernate.Cfg.Configuration BuildConfiguration(DataServiceParameters dataServiceParameters);
        IPersistenceConfigurer GetPersistenceConfigurer(bool createDatabase);
    }
}
