using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Services.Models
{
    public class User
    {
        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual int Age { get; set; }

        public virtual int Height { get; set; }

        public virtual int Weight { get; set; }

        public virtual bool IsActive { get; set; }

        private string _avatarPath;

        private const string DefaultAvatarPath = "/Gymnastika.Infrastructure;component/Images/defaultavatar.png";

        public virtual string AvatarPath
        {
            get
            {
                return string.IsNullOrEmpty(_avatarPath) ? DefaultAvatarPath : _avatarPath;
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
