using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.ProjectResources.Properties;
using Gymnastika.Common.Repositories;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string ErrorString { get; private set; }

        public UserModel Register(UserModel user)
        {
            if (!ValidateUser(user))
                return null;

            UserModel savedUser = _userRepository.Get(user.UserName);
            if (savedUser == null)
            {
                ErrorString = null;
                _userRepository.Add(user);
                return user;
            }

            ErrorString = Resources.DuplicateUserName;

            return null;
        }

        public bool LogOn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                ErrorString = Resources.InvalidUserName;
                return false;
            }

            UserModel savedUser = _userRepository.Get(userName);
            if (savedUser != null)
            {
                if (ComparePassword(password, savedUser.Password))
                {
                    savedUser.IsActive = true;
                    _userRepository.Update(savedUser);
                    return true;
                }

                ErrorString = Resources.InvalidPassword;
                return false;
            }

            ErrorString = Resources.InvalidUserName;

            return false;
        }

        public bool LogOut(string userName)
        {
            UserModel user = _userRepository.Get(userName);

            if (user == null)
            {
                ErrorString = Resources.InvalidUserName;
                return false;
            }

            if (user.IsActive)
            {
                user.IsActive = false;
                _userRepository.Update(user);
            }

            return true;
        }

        private bool ComparePassword(string actual, string expected)
        { 
            if(string.IsNullOrEmpty(actual) && string.IsNullOrEmpty(expected))
                return true;

            return actual == expected;
        }

        private bool ValidateUser(UserModel user)
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
                ErrorString = Resources.InvalidUserName;
                validateResult = false;
            }

            return validateResult;
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public UserModel GetUser(string userName)
        {
            return _userRepository.Get(userName);
        }

        public UserModel GetUser(int id)
        {
            return _userRepository.Get(id);
        }

        public void Update(UserModel u)
        {
            _userRepository.Update(u);
        }
    }
}
