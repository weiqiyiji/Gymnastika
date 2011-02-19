using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Services.Models;

namespace Gymnastika.DesignData
{
    public class StartupViewModel_Design
    {
        public UserCollection RegisteredUsers { get; set; }

        public User SelectedUser { get; set; }
    }
}
