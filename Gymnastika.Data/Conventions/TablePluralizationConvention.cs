using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Gymnastika.Common.Utils;

namespace Gymnastika.Data.Conventions
{
    public class TablePluralizationConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(Inflector.Pluralize(instance.EntityType.Name));
        }
    }
}
