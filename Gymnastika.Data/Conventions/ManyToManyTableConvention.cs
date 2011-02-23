using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Gymnastika.Data.Conventions
{
    /// <summary>
    /// This is used for many-to-many relationship mapping,
    /// if there's two tables "Customers" and "Products", 
    /// then the medium table name is "Customer_Product_Relations",
    /// the table names are in alphabetic order
    /// </summary>
    public class ManyToManyTableConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.All();
            string theSide = instance.EntityType.Name;
            string theOtherSide = instance.OtherSide.EntityType.Name;

            if(theSide.CompareTo(theOtherSide) < 0)
            {
                instance.Table(theSide + "_" + theOtherSide + "_Relations");
            }
            else
            {
                instance.Table(theOtherSide + "_" + theSide + "_Relations");
            }
            
        }
    }
}
