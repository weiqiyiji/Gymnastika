using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Gymnastika.Controls.Behaviors
{
    public class CircleProvider : GeometryProvider
    {
        public static int TopLeft = 1;
        public static int TopRight = 2;
        public static int BottomLeft = 4;
        public static int BottomRight = 8;

        public int Corner { get; set; }

        public override Geometry GetGeometry(FrameworkElement target)
        {
            var width = target.ActualWidth;
            var height = target.ActualHeight;

            var radius = Math.Min(width, height) / 2;
            var size = new Size(radius, radius);

            GeometryGroup group = new GeometryGroup();
            group.FillRule = FillRule.Nonzero;
            group.Children.Add(
                new EllipseGeometry(new Point(radius, radius), radius, radius));

            if ((Corner & TopLeft) == TopLeft)
            {
                group.Children.Add(
                    new RectangleGeometry(new Rect(new Point(0, 0), size)));
            }

            if ((Corner & TopRight) == TopRight)
            {
                group.Children.Add(
                    new RectangleGeometry(new Rect(new Point(radius, 0), size)));
            }

            if ((Corner & BottomLeft) == BottomLeft)
            {
                group.Children.Add(
                    new RectangleGeometry(new Rect(new Point(0, radius), size)));
            }

            if ((Corner & BottomRight) == BottomRight)
            {
                group.Children.Add(
                    new RectangleGeometry(new Rect(new Point(radius, radius), size)));
            }

            return group;
        }
    }
}
