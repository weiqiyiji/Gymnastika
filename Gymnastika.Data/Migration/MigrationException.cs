using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Migration
{
    public class MigrationException : Exception
    {
        public MigrationException(string message)
            : base(message)
        {
        }
    }
}
