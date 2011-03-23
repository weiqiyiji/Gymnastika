using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Gymnastika.Sync.Models
{
    [DataContract(Namespace = "http://tinyurl/Users")]
    public class User
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string UserName { get; set; }

        [DataMember]
        public virtual string Password { get; set; }

        [DataMember]
        public virtual Gender Gender { get; set; }

        [DataMember]
        public virtual int Age { get; set; }

        [DataMember]
        public virtual int Height { get; set; }

        [DataMember]
        public virtual int Weight { get; set; }

        [DataMember]
        public virtual bool IsActive { get; set; }

        [DataMember]
        public virtual string AvatarPath { get; set; }
    }

    public enum Gender
    { 
        Male = 0,
        Female = 1
    }
}
