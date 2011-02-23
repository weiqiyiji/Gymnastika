using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Gymnastika.Data.Conventions
{
    /// <summary>
    /// We should use int as column data type when mapping to Enum type
    /// </summary>
    public class EnumConvention : IUserTypeConvention
    {
        #region IConventionAcceptance<IPropertyInspector> Members

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum);
        }

        #endregion

        #region IConvention<IPropertyInspector,IPropertyInstance> Members

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }

        #endregion
    }
}
