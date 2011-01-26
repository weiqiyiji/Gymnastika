using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.UserManagement.Tests
{
    public class InMemoryUserRepository : IUserRepository
    {
        private IDictionary<Guid, User> _userDb;

        public InMemoryUserRepository()
        {
            _userDb = new Dictionary<Guid, User>();
        }

        #region IUserRepository Members

        public User Get(Guid id)
        {
            return _userDb[id];
        }

        public User Get(string userName)
        {
            return _userDb.SingleOrDefault(pair => pair.Value.UserName == userName).Value;
        }

        public bool Add(User user)
        {
            if (Get(user.UserName) != null)
                return false;

            _userDb.Add(user.Id, user);
            return true;
        }

        public bool Update(User user)
        {
            User savedUser = Get(user.Id);
            if (savedUser != null)
            {
                _userDb[user.Id] = user;
                return true;
            }

            return false;
        }

        public bool Delete(Guid id)
        {
            User savedUser = Get(id);
            if (savedUser != null)
            {
                _userDb.Remove(id);
                return true;
            }

            return false;
        }

        public IEnumerable<User> GetAll()
        {
            return _userDb.Values;
        }

        #endregion
    }
}
