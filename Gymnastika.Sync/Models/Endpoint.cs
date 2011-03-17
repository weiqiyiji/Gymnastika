using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class Endpoint
    {
        public virtual int Id { get; set; }
        public virtual string Uri { get; set; }
        public virtual string Type { get; set; }
    }
}