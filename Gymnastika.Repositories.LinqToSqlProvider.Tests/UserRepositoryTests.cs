using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.Common.Models;

namespace Gymnastika.Repositories.LinqToSqlProvider.Tests
{
    [TestFixture]
    public class UserRepositoryTests : DatabaseEnabledTests
    {
        UserModel _userModel;
        UserRepository _repository;

        [SetUp]
        public void Init()
        {
            _repository = new UserRepository(GetDataContext());
            _userModel = new UserModel
            {
                Id = 0,
                UserName = "UserName",
                Password = "Password",
                Age = 1,
                IsActive = true,
                Height = 12,
                Weight = 12,
                Gender = Gender.Female
            };
        }

        [TearDown]
        public void TearDown()
        {
            GymnastikaDataContext context = GetDataContext();
            context.Users.DeleteAllOnSubmit(context.Users.Select(u => u));
            context.SubmitChanges();
        }

        [Test]
        public void AddUser()
        {
            _repository.Add(_userModel);

            GymnastikaDataContext context = GetDataContext();
            User u = context.Users.FirstOrDefault(ur => ur.UserName == "UserName");

            Assert.That(_userModel.Id, Is.AtLeast(1));
            Assert.That(u, Is.Not.Null);            
            Assert.That(u.Id, Is.AtLeast(1));
            Assert.That(u.Password, Is.EqualTo("Password"));
            Assert.That(u.Age, Is.EqualTo(1));
            Assert.That(u.Height, Is.EqualTo(12));
            Assert.That(u.Weight, Is.EqualTo(12));
            Assert.That(u.IsActive, Is.EqualTo(true));
        }

        [Test]
        public void AddDuplicateUser()
        {
            bool result = _repository.Add(_userModel);
            Assert.That(result, Is.True);

            result = _repository.Add(_userModel);
            Assert.That(result, Is.False);
        }

        [Test]
        public void UpdateUser()
        {
            PrepareUser();

            UserModel userModel = Get(u => u.UserName == "UserName");
            Assert.That(userModel.Age, Is.EqualTo(1));

            userModel.Age = 2;
            _repository.Update(userModel);
            userModel = Get(u => u.UserName == "UserName");

            Assert.That(userModel.Age, Is.EqualTo(2));
        }

        [Test]
        public void DeleteUser()
        {
            PrepareUser();

            UserModel userModel = Get(u => u.UserName == "UserName");
            bool result = _repository.Delete(userModel.Id);
            int count = GetDataContext().Users.Count(u => u.Id == userModel.Id);

            Assert.That(result, Is.True);
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteUnexistsUser()
        {
            bool result = _repository.Delete(1);

            Assert.That(result, Is.False);
        }

        private void PrepareUser()
        {
            var context = GetDataContext();
            context.Users.InsertOnSubmit(Project(_userModel));
            context.SubmitChanges();
        }

        private UserModel Get(Func<User, bool> predicate)
        {
            return GetDataContext().Users
                    .Where(predicate)
                    .Select(Project)
                    .FirstOrDefault();
        }

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
                Weight = u.Weight
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
                Weight = u.Weight
            };
        }
    }
}
