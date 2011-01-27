using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Services;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Repositories
{
    public interface IUserRepository
    {
        UserModel Get(int id);
        UserModel Get(string userName);
        IEnumerable<UserModel> GetAll();
        bool Add(UserModel user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>false if user does not exists</returns>
        bool Update(UserModel user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>false if user does not exists</returns>
        bool Delete(int id);
    }
}
