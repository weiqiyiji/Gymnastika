using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.UserManagement
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public bool IsActive { get; set; }
    }

    public enum Gender
    { 
        Male,
        Female
    }
}
