using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Gymnastika.Data.Conventions;

namespace Gymnastika.Services.Models
{
    [DataContract(Namespace = "http://tinyurl/Users")]
    [GeneratedByAssigned]
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

        private string _avatarPath;

        private const string DefaultAvatarPath = "Images/defaultavatar.png";

        [DataMember]
        public virtual string AvatarPath
        {
            get
            {
                bool isAvatarExists = !string.IsNullOrEmpty(_avatarPath) && File.Exists(_avatarPath);

                return !isAvatarExists ? DefaultAvatarPath : _avatarPath;
            }
            set
            {
                _avatarPath = value;
            }
        }
    }

    public enum Gender
    { 
        Male = 0,
        Female = 1
    }
}
