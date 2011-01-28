using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Repositories;
using Gymnastika.Common.Services;
using Gymnastika.Common.Models;

namespace Gymnastika.Tests.Support
{
    public class InMemoryUserRepository : IUserRepository
    {
        private IDictionary<int, UserModel> _userDb;

        public InMemoryUserRepository()
        {
            _userDb = new Dictionary<int, UserModel>();
        }

        #region IUserRepository Members

        public UserModel Get(int id)
        {
            return _userDb[id];
        }

        public UserModel Get(string userName)
        {
            return _userDb.SingleOrDefault(pair => pair.Value.UserName == userName).Value;
        }

        public bool Add(UserModel user)
        {
            if (Get(user.UserName) != null)
                return false;

            _userDb.Add(user.Id, user);
            return true;
        }

        public bool Update(UserModel user)
        {
            UserModel savedUser = Get(user.Id);
            if (savedUser != null)
            {
                _userDb[user.Id] = user;
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            UserModel savedUser = Get(id);
            if (savedUser != null)
            {
                _userDb.Remove(id);
                return true;
            }

            return false;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _userDb.Values;
        }

        #endregion
    }
}
