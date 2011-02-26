using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Gymnastika.Widgets.Tests
{
    [TestFixture]
    public class WidgetContainerTests
    {
        [Test]
        public void WidgetHostsShouldNotNull()
        {
            IWidgetContainer container = new WidgetContainer();

            Assert.That(container.WidgetHosts, Is.Not.Null);
        }

        [Test]
        public void What()
        {
            IWidgetContainer container = new WidgetContainer();
            container.WidgetHosts.Add(new MockWidgetHost());

            Assert.That(container.WidgetHosts, Is.Not.Null);
        }
    }
}
