using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Gymnastika.Phone.Transitions
{
    public static class FadeRollOut
    {
        public static ITransition GetTransition(UIElement element)
        {
            Storyboard sb = new Storyboard();
            LinearGradientBrush op = new LinearGradientBrush();
            GradientStop start = new GradientStop()
            {
                Offset = 0,
                Color = Color.FromArgb(0, 255, 255, 255)
            };
            GradientStop center = new GradientStop()
            {
                Offset = 0.01,
                Color = Color.FromArgb(255, 255, 255, 255)
            };
            op.GradientStops.Add(start);
            op.GradientStops.Add(center);
            op.GradientStops.Add(new GradientStop()
            {
                Offset = 1,
                Color = Colors.White
            });
            element.OpacityMask = op;
            DoubleAnimation ani = new DoubleAnimation();
            DoubleAnimation ani2 = new DoubleAnimation();
            Storyboard.SetTarget(ani, center);
            Storyboard.SetTargetProperty(ani, new PropertyPath(GradientStop.OffsetProperty));
            Storyboard.SetTarget(ani2, start);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(GradientStop.OffsetProperty));
            ani2.From = 0;
            ani2.To = 1;

            ani.From = 0.001;
            ani.To = 1;

            ani.Duration = TimeSpan.FromSeconds(.3);
            ani2.Duration = TimeSpan.FromSeconds(.3);
            ani2.BeginTime = TimeSpan.FromSeconds(.2);
            sb.Children.Add(ani);
            sb.Children.Add(ani2);
            return new Transition(element, sb);
        }
    }
}
