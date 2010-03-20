using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace Gymnastika.Controls
{
    public class AnimatedProgressBar : ProgressBar
    {
        public AnimatedProgressBar()
        {
            DefaultStyleKey = typeof(AnimatedProgressBar);
        }

        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double), typeof(AnimatedProgressBar), new PropertyMetadata(1d));


        
        public double TargetValue
        {
            get { return (double)GetValue(TargetValueProperty); }
            set { SetValue(TargetValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetValueProperty =
            DependencyProperty.Register("TargetValue", typeof(double), typeof(AnimatedProgressBar), new PropertyMetadata(TargetValueChanged));

        static void TargetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AnimatedProgressBar sender = d as AnimatedProgressBar;
            double value = (double)e.NewValue;
            sender.BeginWidthAnimation();
        }


        private void BeginWidthAnimation()
        {
            double dt = (Math.Abs(TargetValue - Value) / Maximum);
            double dur = Math.Sqrt(dt) * (Duration-0.4d) + 0.4d ;
            DoubleAnimation valueAni = new DoubleAnimation(ToRange(TargetValue), TimeSpan.FromSeconds(dur));
            valueAni.EasingFunction = new ElasticEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut, Oscillations = 0, Springiness = 0.5 };
            BeginAnimation(ValueProperty,valueAni);
        }

        double ToRange(double value)
        {
            if (value < this.Minimum)
                value = Minimum;
            if (value > Maximum)
                value = Maximum;
            return value;
        }
    }
}
