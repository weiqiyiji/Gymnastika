using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Models
{
    public class MigrationRecord
    {
        public virtual int Id { get; set; }
        public virtual string Version { get; set; }
        public virtual string TableName { get; set; }
    }
}
