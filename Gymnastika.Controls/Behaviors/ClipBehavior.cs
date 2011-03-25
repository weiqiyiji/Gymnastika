using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Gymnastika.Controls.Behaviors
{
    public class ClipBehavior
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ClipBehavior), new UIPropertyMetadata(new CornerRadius(), OnSetCornerRadius));

        public static void OnSetCornerRadius(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement target = (FrameworkElement)sender;
            target.SizeChanged += new SizeChangedEventHandler(target_SizeChanged);
        }

        private static void target_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateClip((FrameworkElement)sender);
        }

        private static bool IsNeedToClip(CornerRadius cornerRadius)
        {
            return cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0 || cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0;
        }

        private static double GetMinRadius(double radius1, double radius2)
        {
            return (radius1 == 0 || radius2 == 0) ? Math.Max(radius1, radius2) : Math.Min(radius1, radius2);
        }

        private static void UpdateClip(FrameworkElement target)
        {
            var height = target.ActualHeight;
            var width = target.ActualWidth;

            var cornerRadius = GetCornerRadius(target);

            if (IsNeedToClip(cornerRadius))
            {
                GeometryGroup geometryGroup = new GeometryGroup();
                geometryGroup.FillRule = FillRule.Nonzero;

                var topMaxHeight = Math.Max(cornerRadius.TopLeft, cornerRadius.TopRight);
                var topMinHeight = GetMinRadius(cornerRadius.TopLeft, cornerRadius.TopRight);
                var bottomMaxHeight = Math.Max(cornerRadius.BottomLeft, cornerRadius.BottomRight);
                var bottomMinHeight = GetMinRadius(cornerRadius.BottomLeft, cornerRadius.BottomRight);

                //Add the top rectangle
                if(topMinHeight > 0)
                {
                    var topRectWidth = width - cornerRadius.TopLeft - cornerRadius.TopRight;
                    geometryGroup.Children.Add(new RectangleGeometry(
                        new Rect(new Point(cornerRadius.TopLeft, 0),
                                 new Size(topRectWidth, topMinHeight))));
                }

                //Add the extra space
                if(topMaxHeight - topMinHeight > 0)
                {
                    var topSpace = topMaxHeight - topMinHeight;
                    var rectLeft = cornerRadius.TopLeft > cornerRadius.TopRight ? cornerRadius.TopLeft : 0;
                    var rectWidth = width - topMaxHeight;

                    geometryGroup.Children.Add(
                        new RectangleGeometry(
                            new Rect(new Point(rectLeft, topMinHeight),
                                     new Size(rectWidth, topSpace))));
                }

                //Add the lefttop circle
                if (cornerRadius.TopLeft > 0)
                {
                    var radius = cornerRadius.TopLeft;

                    geometryGroup.Children.Add(new EllipseGeometry(new Point(radius, radius), radius, radius));
                }

                //Add the righttop circle
                if (cornerRadius.TopRight > 0)
                {
                    var radius = cornerRadius.TopRight;

                    geometryGroup.Children.Add(
                        new EllipseGeometry(new Point(width - radius, radius), radius, radius));
                }

                //Add the center rectangle
                if(height - topMaxHeight - bottomMaxHeight > 0)
                {
                    geometryGroup.Children.Add(
                        new RectangleGeometry(
                            new Rect(new Point(0, topMaxHeight), 
                                     new Size(width, height - topMaxHeight - bottomMaxHeight))));
                }

                //Add the leftbottom circle
                if(cornerRadius.BottomLeft > 0)
                {
                    var radius = cornerRadius.BottomLeft;

                    geometryGroup.Children.Add(
                        new EllipseGeometry(new Point(radius, height - radius), radius, radius));
                }

                //Add the rightbottom circlr
                if(cornerRadius.BottomRight > 0)
                {
                    var radius = cornerRadius.BottomRight;

                    geometryGroup.Children.Add(
                        new EllipseGeometry(new Point(width - radius, height - radius), radius, radius));
                }

                //Add the bottom rectangle
                if(bottomMinHeight > 0)
                {
                    var rectWidth = width - cornerRadius.BottomLeft - cornerRadius.BottomRight;
                    geometryGroup.Children.Add(
                        new RectangleGeometry(
                            new Rect(new Point(cornerRadius.BottomLeft, height - bottomMinHeight), 
                                     new Size(rectWidth, bottomMinHeight))));
                }

                //Add the bottom extra space
                if(bottomMaxHeight - bottomMinHeight > 0)
                {
                    var space = bottomMaxHeight - bottomMinHeight;
                    var rectLeft = cornerRadius.BottomLeft > cornerRadius.BottomRight ? cornerRadius.BottomLeft : 0;
                    var rectWidth = width - bottomMaxHeight;

                    geometryGroup.Children.Add(
                        new RectangleGeometry(
                            new Rect(new Point(rectLeft, bottomMinHeight),
                                     new Size(rectWidth, space))));
                }

                target.Clip = geometryGroup;
            }
        }
    }
}
