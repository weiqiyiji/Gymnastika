using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Repositories;
using Gymnastika.Common.Models;

namespace Gymnastika.Repositories.LinqToSqlProvider
{
    public class UserRepository : IUserRepository
    {
        private GymnastikaDataContext DataContext { get; set; }

        public UserRepository(GymnastikaDataContext context)
        {
            DataContext = context;
        }

        #region IUserRepository Members

        public UserModel Get(int id)
        {
            User user = DataContext.Users.FirstOrDefault(u => u.Id == id);
            return user != null ? Project(user) : null;
        }

        public UserModel Get(string userName)
        {
            User user = DataContext.Users.FirstOrDefault(u => u.UserName == userName);
            return user != null ? Project(user) : null;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return DataContext.Users.Select(u => Project(u));
        }
            
        public bool Add(UserModel user)
        {
            User u = DataContext.Users.FirstOrDefault(ur => ur.UserName == user.UserName);
            if (u != null) return false;

            u = Project(user);
            DataContext.Users.InsertOnSubmit(u);
            DataContext.SubmitChanges();

            user.Id = u.Id;

            return true;
        }

        public bool Update(UserModel u)
        {
            User user = DataContext.Users.FirstOrDefault(ur => ur.Id == u.Id);
            if (user == null) return false;

            user.UserName = u.UserName;
            user.Password = u.Password;
            user.Age = u.Age;
            user.IsActive = u.IsActive;
            user.Height = u.Height;
            user.Weight = u.Weight;
            user.Gender = u.Gender == Gender.Male;
            DataContext.SubmitChanges();

            return true;
        }

        public bool Delete(int id)
        {
            User user = DataContext.Users.FirstOrDefault(ur => ur.Id == id);
            if (user == null) return false;

            DataContext.Users.DeleteOnSubmit(user);
            DataContext.SubmitChanges();

            return true;
        }

        #endregion

        private UserModel Project(User u)
        {
            return new UserModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password,
                Age = u.Age,
                IsActive = u.IsActive,
                Height = u.Height,
                Weight = u.Weight,
                Gender = u.Gender ? Gender.Male : Gender.Female
            };
        }

        private User Project(UserModel u)
        {
            return new User
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password,
                Age = u.Age,
                IsActive = u.IsActive,
                Height = u.Height,
                Weight = u.Weight,
                Gender = u.Gender == Gender.Male
            };
        }
    }
}
