using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.ViewModels;

namespace Gymnastika.Views
{
    public interface IUserProfileView
    {
        UserProfileViewModel Model { get; set; }
        void Show();
    }
}
