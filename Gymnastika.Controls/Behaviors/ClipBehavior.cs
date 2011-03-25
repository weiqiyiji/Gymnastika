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
        public static double GetCornerRadius(FrameworkElement obj)
        {
            return (double)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(FrameworkElement obj, double value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(double), typeof(ClipBehavior), new UIPropertyMetadata(0.0, OnSetCornerRadius));

        public static void OnSetCornerRadius(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement target = (FrameworkElement)sender;
            target.SizeChanged += new SizeChangedEventHandler(target_SizeChanged);
        }

        private static void target_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateClip((FrameworkElement)sender);
        }

        private static void UpdateClip(FrameworkElement target)
        {
            var height = target.ActualHeight;
            var width = target.ActualWidth;

            var cornerRadius = GetCornerRadius(target);

            if (cornerRadius > 0)
            {
                GeometryGroup geometryGroup = new GeometryGroup();
                geometryGroup.FillRule = FillRule.Nonzero;
                var smallRectWidth = width - cornerRadius;
                var smallRectHeight = cornerRadius;

                //Add the lefttop rectangle
                var leftTopRect = new RectangleGeometry(
                    new Rect(new Point(0, 0), new Size(smallRectWidth, smallRectHeight)));

                geometryGroup.Children.Add(leftTopRect);

                //Add the righttop circle
                var rightTopCircle = new EllipseGeometry(
                    new Point(smallRectWidth, cornerRadius), cornerRadius, cornerRadius);

                geometryGroup.Children.Add(rightTopCircle);

                //Add the center rectangle
                var centerRect = new RectangleGeometry(
                    new Rect(new Point(0, smallRectHeight), new Size(width, height - 2 * smallRectHeight)));

                geometryGroup.Children.Add(centerRect);

                //Add the leftbottom circle
                var leftBottomCircle = new EllipseGeometry(
                    new Point(cornerRadius, height - smallRectHeight), cornerRadius, cornerRadius);

                geometryGroup.Children.Add(leftBottomCircle);

                //Add the rightbottom rectangle
                var rightBottomRectangle = new RectangleGeometry(
                    new Rect(new Point(cornerRadius, height - smallRectHeight), new Size(smallRectWidth, smallRectHeight)));

                geometryGroup.Children.Add(rightBottomRectangle);
                target.Clip = geometryGroup;
            }
        }
    }
}
