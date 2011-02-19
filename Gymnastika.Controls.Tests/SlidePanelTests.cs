using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Windows.Controls;

namespace Gymnastika.Controls.Tests
{
    [TestFixture]
    public class SlidePanelTests
    {
        MockSlidePanel _slidePanel;

        [SetUp]
        public void SetUp()
        {
            _slidePanel = new MockSlidePanel();
            _slidePanel.Children.Add(new NumberControl(0));
            _slidePanel.Children.Add(new NumberControl(1));
            _slidePanel.Children.Add(new NumberControl(2));
            _slidePanel.Children.Add(new NumberControl(3));
            _slidePanel.Children.Add(new NumberControl(4));
        }

        [TearDown]
        public void TearDown()
        {
            _slidePanel = null;
        }

        [Test]
        [STAThread]
        public void CalculateShortestDistance_FromLeftToRightInCircle()
        {
            int shortestDistance = _slidePanel.CallCalculateShortestDistance(0, 4);

            Assert.That(shortestDistance, Is.EqualTo(1));
        }

        [Test]
        [STAThread]
        public void CalculateShortestDistance_FromLeftToRightInLine()
        {
            int shortestDistance = _slidePanel.CallCalculateShortestDistance(2, 4);

            Assert.That(shortestDistance, Is.EqualTo(-2));
        }

        [Test]
        [STAThread]
        public void CalculateShortestDistance_FromRightToLeftInLine()
        {
            int shortestDistance = _slidePanel.CallCalculateShortestDistance(4, 2);

            Assert.That(shortestDistance, Is.EqualTo(2));
        }

        [Test]
        [STAThread]
        public void CalculateShortestDistance_LeftEqualToRight_ReturnsZero()
        {
            int shortestDistance = _slidePanel.CallCalculateShortestDistance(0, 0);

            Assert.That(shortestDistance, Is.EqualTo(0));
        }

        [Test]
        [STAThread]
        public void CalculateShortestDistance_FromRightToLeftInCircle()
        {
            int shortestDistance = _slidePanel.CallCalculateShortestDistance(4, 0);

            Assert.That(shortestDistance, Is.EqualTo(-1));
        }
    }

    internal class NumberControl : Control
    {
        public int Number { get; set; }
        public NumberControl(int number) 
        {
            Number = number;
        }
    }

    internal class MockSlidePanel : SlidePanel
    {
        public int CallCalculateShortestDistance(int from, int to)
        { 
            return base.CalculateShortestDistance(from, to);
        }
    }
}
