using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Gymnastika.Data.Conventions
{
    public class UnsavedIdConvention : IIdConvention
    {
        #region IConvention<IIdentityInspector,IIdentityInstance> Members

        public void Apply(IIdentityInstance instance)
        {
            //instance.GeneratedBy.Identity();
        }

        #endregion
    }
}
