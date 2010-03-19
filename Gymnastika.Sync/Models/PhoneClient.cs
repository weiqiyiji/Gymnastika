using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gymnastika.Sync.Models
{
    public class PhoneClient
    {
        public virtual int Id { get; set; }
        public virtual string Uri { get; set; }
    }
}