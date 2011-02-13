using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;

namespace Gymnastika.Data.Configuration
{
    public class AutomappingConfigurationFilter : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace.Contains(".Models");
        }
    }
}
