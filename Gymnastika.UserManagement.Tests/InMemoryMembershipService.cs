using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.UserManagement.Tests
{
    public class InMemoryMembershipService : IMembershipService
    {
        private IList<User> _registeredUsers;

        public InMemoryMembershipService()
        {
            _registeredUsers = new List<User>();
        }

        #region IMembershipService Members

        public MembershipCreateStatus Create(User user)
        {
            if(_registeredUsers.SingleOrDefault(u => u.UserName == user.UserName) != null)
                return MembershipCreateStatus.DuplicateUserName;

            _registeredUsers.Add(user);

            return MembershipCreateStatus.Success;
        }

        public LogOnStatus Validate(string userName, string password)
        {
            return LogOnStatus.Success;
        }

        #endregion
    }
}
