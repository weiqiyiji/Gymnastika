using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Migration
{
    public class MigrationNullException : Exception
    {
        public MigrationNullException(string version) 
            : base(string.Format("Migration version {0} does not exist", version)) { }
    }
}
