using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.Controllers;
using Microsoft.Practices.Unity;
using Gymnastika.Views;
using Microsoft.Practices.Prism.Regions;
using Moq;
using Microsoft.Practices.Prism.Events;

namespace Gymnastika.Tests.Controllers
{
    [TestFixture]
    public class StartupControllerTests
    {
        [Test]
        public void RegisterDependencies()
        {
            IUnityContainer container = new UnityContainer();

            MockStartupController controller = new MockStartupController(container);
            controller.CallRegisterDependencies();

            Assert.That(container.IsRegistered<IStartupView>(), Is.True);
            Assert.That(container.IsRegistered<IMainView>(), Is.True);
        }
    }

    internal class MockStartupController : StartupController
    {
        public MockStartupController(IUnityContainer container) 
            : base(container, new Mock<IRegionManager>().Object)
        { }

        public void CallRegisterDependencies()
        {
            base.RegisterDependencies();
        }

        public void CallSubscribeEvents()
        {
            base.SubscribeEvents();
        }
    }
}
