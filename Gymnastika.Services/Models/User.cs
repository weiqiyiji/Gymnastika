using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        private const string DefaultAvatarPath = "Images/defaultavatar.png";

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
