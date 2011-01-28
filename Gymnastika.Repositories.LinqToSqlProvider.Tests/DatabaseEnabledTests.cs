using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Repositories.LinqToSqlProvider.Tests
{
    public class DatabaseEnabledTests
    {
        protected GymnastikaDataContext GetDataContext()
        {
            return new GymnastikaDataContext("Data Source=Gymnastika.sdf;Persist Security Info=False;");
        }
    }
}
