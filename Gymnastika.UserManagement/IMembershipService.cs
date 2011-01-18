using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.UserManagement
{
    public interface IMembershipService
    {
        MembershipCreateStatus Create(User user);
        LogOnStatus Validate(string userName, string password);
    }

    public enum MembershipCreateStatus
    {
        Success,
        InvalidUserName,
        InvalidPassword,
        InvalidEmail,
        DuplicateUserName,
        DuplicateEmail
    }

    public enum LogOnStatus
    { 
        Success,
        InvalidUserName,
        InvalidPassword
    }
}
