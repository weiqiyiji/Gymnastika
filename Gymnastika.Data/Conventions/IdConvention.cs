using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;

namespace Gymnastika.Data.Conventions
{
    public class IdConvention : IIdConvention
    {
        #region IConvention<IIdentityInspector,IIdentityInstance> Members

        public void Apply(FluentNHibernate.Conventions.Instances.IIdentityInstance instance)
        {
            if (Attribute.IsDefined(instance.EntityType, typeof(GeneratedByAssignedAttribute)))
                instance.GeneratedBy.Assigned();
            else
                instance.Column("Id");
        }

        #endregion
    }
}
