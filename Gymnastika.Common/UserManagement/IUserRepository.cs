using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.UserManagement
{
    public interface IUserRepository
    {
        User Get(Guid id);
        User Get(string userName);
        IEnumerable<User> GetAll();
        bool Add(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>false if user does not exists</returns>
        bool Update(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>false if user does not exists</returns>
        bool Delete(Guid id);
    }
}
