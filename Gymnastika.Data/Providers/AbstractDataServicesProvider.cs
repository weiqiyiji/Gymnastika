using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Cfg;
using Gymnastika.Data.Providers;

namespace Gymnastika.Data.Providers {
    public abstract class AbstractDataServicesProvider : IDataServicesProvider {

        public abstract IPersistenceConfigurer GetPersistenceConfigurer(bool createDatabase);

        public NHibernate.Cfg.Configuration BuildConfiguration(DataServiceParameters parameters)
        {
            var database = GetPersistenceConfigurer(parameters.CreateDatabase);

            FluentConfiguration cfg = Fluently.Configure().Database(database);
            return InnerConfiguration(cfg).BuildConfiguration();
        }

        protected virtual FluentConfiguration InnerConfiguration(FluentConfiguration cfg) { return cfg; }
    }
}