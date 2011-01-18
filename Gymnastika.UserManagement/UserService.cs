using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.ProjectResources.Properties;

namespace Gymnastika.UserManagement
{
    public class UserService
    {
        private IMembershipService _membershipService;

        public UserService(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public string ErrorString { get; private set; }

        public User Register(User user)
        {
            if (!ValidateUser(user))
                return null;

            user.Id = Guid.NewGuid();
            MembershipCreateStatus status = _membershipService.Create(user);

            ErrorString = ErrorCodeToString(status);

            if(status == MembershipCreateStatus.Success)
                return user;

            return null;
        }

        public bool LogOn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                ErrorString = Resources.InvalidUserName;
                return false;
            }

            LogOnStatus status = _membershipService.Validate(userName, password);
            ErrorString = ErrorCodeToString(status);

            return status == LogOnStatus.Success;
        }

        public bool LogOut(string userName)
        {
            return true;
        }

        private bool ValidateUser(User user)
        {
            bool validateResult = true;

            if (user == null)
                throw new ArgumentNullException("user should not be null");

            if (user.Age < 0)
            {
                ErrorString = Resources.InvalidAge;
                validateResult = false;
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                ErrorString = ErrorCodeToString(MembershipCreateStatus.InvalidUserName);
                validateResult = false;
            }

            return validateResult;
        }

        private string ErrorCodeToString(MembershipCreateStatus status)
        {
            switch (status)
            { 
                case MembershipCreateStatus.InvalidUserName:
                    return Resources.InvalidUserName;

                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.DuplicateUserName;
            }

            return null;
        }

        private string ErrorCodeToString(LogOnStatus status)
        {
            switch (status)
            {
                case LogOnStatus.InvalidUserName:
                    return Resources.InvalidUserName;

                case LogOnStatus.InvalidPassword:
                    return Resources.InvalidPassword;
            }

            return null;
        }
    }
}
