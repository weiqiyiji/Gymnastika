using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Data.Providers;
using FluentNHibernate.Cfg;

namespace Gymnastika.Sync.Infrastructure
{
    public class CustomSqlCeDataServicesProvider : SqlCeDataServicesProvider
    {
        public CustomSqlCeDataServicesProvider(string dataFolder, string dbName) : base(dataFolder, dbName) { }

        protected override FluentConfiguration InnerConfiguration(FluentConfiguration cfg)
        {
            FluentConfiguration configuration = base.InnerConfiguration(cfg).Mappings(
                x => 
                {
                    var autoMapping = x.AutoMappings.First();
                    autoMapping = autoMapping.UseOverridesFromAssemblyOf<ConnectionAutoMappingOverride>();
#if DEBUG
                    x.AutoMappings.ExportTo(HttpContext.Current.Server.MapPath("~/"));
#endif
                });

            return configuration;
        }
    }
}