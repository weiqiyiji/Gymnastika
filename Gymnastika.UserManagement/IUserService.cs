using System;
using System.Collections.Generic;
namespace Gymnastika.UserManagement
{
    public interface IUserService
    {
        string ErrorString { get; }
        User GetUser(Guid id);
        User GetUser(string userName);
        bool LogOn(string userName, string password);
        bool LogOut(string userName);
        User Register(User user);
        void Update(User u);
        IEnumerable<User> GetAllUsers();
    }
}
