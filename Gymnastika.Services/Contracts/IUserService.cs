using System;
using System.Collections.Generic;
using Gymnastika.Services.Models;

namespace Gymnastika.Services.Contracts
{
    public interface IUserService
    {
        string ErrorString { get; }
        User GetUser(int id);
        User GetUser(string userName);
        bool LogOn(string userName, string password);
        bool LogOut(string userName);
        User Register(User user);
        void Update(User u);
        IEnumerable<User> GetAllUsers();
    }
}
