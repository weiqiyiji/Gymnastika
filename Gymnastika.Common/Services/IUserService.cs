using System;
using System.Collections.Generic;
using Gymnastika.Common.Models;
namespace Gymnastika.Common.Services
{
    public interface IUserService
    {
        string ErrorString { get; }
        UserModel GetUser(int id);
        UserModel GetUser(string userName);
        bool LogOn(string userName, string password);
        bool LogOut(string userName);
        UserModel Register(UserModel user);
        void Update(UserModel u);
        IEnumerable<UserModel> GetAllUsers();
    }
}
