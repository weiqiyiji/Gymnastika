using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class Connection
    {
        public virtual int Id { get; set; }
        public virtual Endpoint Source { get; set; }
        public virtual Endpoint Target { get; set; }
    }
}