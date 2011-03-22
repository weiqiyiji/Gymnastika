using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Data;
using Gymnastika.ProjectResources.Properties;
using System.Net;
using Gymnastika.Sync.Models;

namespace Gymnastika.Sync
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserProfileService
    {
        private IRepository<User> _userRepository;

        public UserProfileService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [WebInvoke(UriTemplate = "register", Method = "POST")]
        public string Register(User user)
        {
            ValidateUser(user);

            User savedUser = GetUserByName(user.UserName);
            if (savedUser == null)
            {
                _userRepository.Create(user);
                SetStatusCode(HttpStatusCode.Created);
                return user.Id.ToString();
            }

            throw new WebFaultException<string>(Resources.DuplicateUserName, HttpStatusCode.BadRequest);
        }

        [WebInvoke(UriTemplate = "logon", Method = "POST")]
        public int LogOn(LogOnInfo logOnInfo)
        {
            string userName = logOnInfo.UserName;
            string password = logOnInfo.Password;

            if (string.IsNullOrEmpty(userName))
            {
                throw new WebFaultException<string>(Resources.InvalidUserName, HttpStatusCode.BadRequest);
            }

            User savedUser = GetUserByName(userName);
            if (savedUser != null)
            {
                if (ComparePassword(password, savedUser.Password))
                {
                    savedUser.IsActive = true;
                    _userRepository.Update(savedUser);
                    return savedUser.Id;
                }

                throw new WebFaultException<string>(Resources.InvalidPassword, HttpStatusCode.BadRequest);
            }

            throw new WebFaultException<string>(Resources.InvalidUserName, HttpStatusCode.BadRequest);
        }

        [WebGet(UriTemplate = "logout?username={userName}")]
        public void LogOut(string userName)
        {
            User user = GetUserByName(userName);

            if (user == null)
            {
                throw new WebFaultException<string>(Resources.InvalidUserName, HttpStatusCode.BadRequest);
            }

            if (user.IsActive)
            {
                user.IsActive = false;
                _userRepository.Update(user);
            }
        }

        [WebGet(UriTemplate = "get_by_name?username={userName}")]
        public User GetUserByName(string userName)
        {
            return _userRepository.Get(u => u.UserName == userName);
        }

        [WebGet(UriTemplate = "get_by_id?id={id}")]
        public User GetUserById(int id)
        {
            return _userRepository.Get(id);
        }

        [WebInvoke(UriTemplate = "update", Method = "POST")]
        public void Update(User u)
        {
            _userRepository.Update(u);
        }

        private bool ComparePassword(string actual, string expected)
        { 
            if(string.IsNullOrEmpty(actual) && string.IsNullOrEmpty(expected))
                return true;

            return actual == expected;
        }

        private void ValidateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user should not be null");

            if (user.Age < 0)
            {
                throw new WebFaultException<string>(
                    Resources.InvalidAge, HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new WebFaultException<string>(
                    Resources.InvalidUserName, HttpStatusCode.BadRequest);
            }
        }

        private void SetStatusCode(HttpStatusCode statusCode)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
        }
    }
}