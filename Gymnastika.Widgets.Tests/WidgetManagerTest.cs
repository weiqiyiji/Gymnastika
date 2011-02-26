using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Moq;
using NUnit.Framework;

namespace Gymnastika.Widgets.Tests
{
    [TestFixture]
    public class WidgetManagerTest
    {
        private Mock<IWidgetContainerBehaviorFactory> _mockWidgetHostFactory;

        [SetUp]
        public void SetUp()
        {
            _mockWidgetHostFactory = new Mock<IWidgetContainerBehaviorFactory>();
        }

        [Test]
        public void Add_Widgets_ShouldContainsNewlyOne()
        {
            _mockWidgetHostFactory
                .Setup(x => x.CreateWidgetHost())
                .Returns(new MockWidgetHost());
            var widget = new MockWidget();
            WidgetManager manager = new WidgetManager(new MockWidgetContainer(), _mockWidgetHostFactory.Object);
            manager.Add(widget);

            Assert.That(manager.Widgets, Is.Not.Null);
            Assert.That(manager.Widgets.Count, Is.EqualTo(1));
            Assert.That(manager.Widgets.First(), Is.EqualTo(widget));
        }

        [Test]
        public void Add_AfterAdd_WidgetShouldInitialized()
        {
            _mockWidgetHostFactory
                .Setup(x => x.CreateWidgetHost())
                .Returns(new MockWidgetHost());
            var widget = new MockWidget();
            WidgetManager manager = new WidgetManager(new MockWidgetContainer(), _mockWidgetHostFactory.Object);
            manager.Add(widget);

            Assert.That(widget.IsInitialized, Is.True);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullWidget_Throws()
        {
            WidgetManager manager = new WidgetManager(new MockWidgetContainer(), _mockWidgetHostFactory.Object);
            manager.Add(null);
        }

        [Test]
        public void Add_WidgetHostShouldNotNull()
        {
            _mockWidgetHostFactory
                .Setup(x => x.CreateWidgetHost())
                .Returns(new MockWidgetHost());

            var widget = new MockWidget();
            WidgetManager manager = new WidgetManager(new MockWidgetContainer(), _mockWidgetHostFactory.Object);
            manager.Add(widget);

            Assert.That(widget.Host, Is.Not.Null);
        }

        [Test]
        public void Add_WidgetHostShouldReferencesWidget()
        {
            var widgetHost = new MockWidgetHost();

            _mockWidgetHostFactory
                .Setup(x => x.CreateWidgetHost())
                .Returns(widgetHost);

            var widget = new MockWidget();
            WidgetManager manager = new WidgetManager(new MockWidgetContainer(), _mockWidgetHostFactory.Object);
            manager.Add(widget);

            Assert.That(widgetHost.Widget, Is.Not.Null);
            Assert.That(widgetHost.Widget, Is.EqualTo(widget));
        }

        [Test]
        public void Add_WidgetContainerShouldContainRelatedWidgetHost()
        {
            var widgetHost = new MockWidgetHost();
            var widgetContainer = new MockWidgetContainer();

            _mockWidgetHostFactory
                .Setup(x => x.CreateWidgetHost())
                .Returns(widgetHost);

            var widget = new MockWidget();
            WidgetManager manager = new WidgetManager(widgetContainer, _mockWidgetHostFactory.Object);
            manager.Add(widget);

            Assert.That(widgetContainer.WidgetHosts.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_WidgetShouldReferencesContainer()
        {
            var widgetHost = new MockWidgetHost();
            var widgetContainer = new MockWidgetContainer();

            _mockWidgetHostFactory
                .Setup(x => x.CreateWidgetHost())
                .Returns(widgetHost);

            var widget = new MockWidget();
            WidgetManager manager = new WidgetManager(widgetContainer, _mockWidgetHostFactory.Object);
            manager.Add(widget);

            Assert.That(widget.Container, Is.Not.Null);
            Assert.That(widget.Container, Is.EqualTo(widgetContainer));
        }
    }

    class MockWidgetContainer : IWidgetContainer
    {
        public MockWidgetContainer()
        {
            WidgetHosts = new List<IWidgetHost>();
        }

        public IList<IWidgetHost> WidgetHosts { get; private set; }
    }

    class MockWidgetHost : IWidgetHost
    {
        public int Id
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Point Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Expand()
        {
            throw new NotImplementedException();
        }

        public void Collapse()
        {
            throw new NotImplementedException();
        }

        public WidgetState State
        {
            get { throw new NotImplementedException(); }
        }

        public IWidget Widget { get; set; }
    }

    class MockWidget : IWidget
    {
        public IWidgetHost Host { get; private set; }
        public IWidgetContainer Container { get; private set; }

        public bool IsInitialized { get; private set; }

        public void Initialize(WidgetContext context)
        {
            IsInitialized = true;
            Host = context.Host;
            Container = context.Container;
        }
    }
}
