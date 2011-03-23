using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gymnastika.ProjectResources.Properties;
using Gymnastika.Services.Models;
using Gymnastika.Data;
using Gymnastika.Services.Contracts;
using Gymnastika.Sync.Communication.Client;
using Microsoft.Http;

namespace Gymnastika.Services.Impl
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;
        private UserProfileService _userProfileService;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _userProfileService = new UserProfileService();
        }

        public string ErrorString { get; private set; }

        public User Register(User user)
        {
            if (!ValidateUser(user))
                return null;

            User savedUser = GetUser(user.UserName);
            if (savedUser == null)
            {
                ErrorString = null;
                ResponseMessage response = _userProfileService.Register(HttpContentExtensions.CreateDataContract<User>(user));

                if (!response.HasError)
                {
                    user.Id = int.Parse(StringHelper.GetPureString(response.Response.Content.ReadAsString()));
                    _userRepository.Create(user);
                    return user;
                }
                else
                {
                    ErrorString = response.ErrorMessage;
                    return null;
                }
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

            User savedUser = GetUser(userName);
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
            User user = GetUser(userName);

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
                ErrorString = Resources.InvalidUserName;
                validateResult = false;
            }

            return validateResult;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.Fetch(u => true);
        }

        public User GetUser(string userName)
        {
            ResponseMessage response = _userProfileService.GetUserByName(userName);

            HttpContent content = response.Response.Content;
            content.LoadIntoBuffer();
            if (string.IsNullOrEmpty(content.ReadAsString())) return null;

            return content.ReadAsDataContract<User>();
        }

        public User GetUser(int id)
        {
            ResponseMessage response = _userProfileService.GetUserById(id.ToString());

            HttpContent content = response.Response.Content;
            content.LoadIntoBuffer();

            if (string.IsNullOrEmpty(content.ReadAsString())) return null;

            return content.ReadAsDataContract<User>();
        }

        public void Update(User u)
        {
            _userProfileService.Update(HttpContentExtensions.CreateDataContract<User>(u));
            _userRepository.Update(u);
        }
    }
}
