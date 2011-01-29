using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.Common.Session;
using Gymnastika.Common.Models;

namespace Gymnastika.Common.Tests.Session
{
    [TestFixture]
    public class SessionManagerTests
    {
        [Test]
        public void AddNewUserModel()
        {
            SessionManager sessionManager = new SessionManager();
            sessionManager.Add(new UserModel { Id = 1 });

            SessionContext context = sessionManager.GetCurrentSession();

            AssertSessionContext(context, 1);
        }

        [Test]
        public void AddTwoUserModels()
        {
            SessionManager sessionManager = new SessionManager();
            sessionManager.Add(new UserModel { Id = 1 });

            SessionContext context = sessionManager.GetCurrentSession();
            AssertSessionContext(context, 1);

            sessionManager.Add(new UserModel { Id = 2 });
            context = sessionManager.GetCurrentSession();
            AssertSessionContext(context, 2);
        }

        [Test]
        public void RemoveUser()
        {
            UserModel user = new UserModel { Id = 1 };
            SessionManager sessionManager = new SessionManager();
            sessionManager.Add(user);

            SessionContext context = sessionManager.GetCurrentSession();
            AssertSessionContext(context, 1);

            sessionManager.Remove(user);
            context = sessionManager.GetCurrentSession();

            Assert.That(context, Is.Null);
        }

        private void AssertSessionContext(SessionContext context, int userId)
        {
            Assert.That(context, Is.Not.Null);
            Assert.That(context.AssociatedUser, Is.Not.Null);
            Assert.That(context.Timestamp, Is.Not.EqualTo(default(DateTime)));
            Assert.That(context.AssociatedUser.Id, Is.EqualTo(userId));
        }
    }
}
