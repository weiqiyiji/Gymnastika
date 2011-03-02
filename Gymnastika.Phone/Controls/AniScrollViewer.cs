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
using System.Collections.Generic;
namespace Gymnastika.Phone.Controls
{
    public class AniScrollViewer : DependencyObject
    {
        private static Dictionary<ScrollViewer, AniScrollViewer> m_anis = new Dictionary<ScrollViewer, AniScrollViewer>();
        public static AniScrollViewer GetAni(ScrollViewer viewer)
        {
            if (m_anis.ContainsKey(viewer))
                return m_anis[viewer];
            else
                return null;
        }
        public static AniScrollViewer SetAni(ScrollViewer viwer)
        {
            if (m_anis.ContainsKey(viwer))
                return m_anis[viwer];
            else
            {
                AniScrollViewer v = new AniScrollViewer() { TargetViewer = viwer };
                m_anis.Add(viwer,v);
                viwer.Unloaded += new RoutedEventHandler(viwer_Unloaded);
                return v;
            }
        }

        static void viwer_Unloaded(object sender, RoutedEventArgs e)
        {
            m_anis.Remove(sender as ScrollViewer);
        }
        public ScrollViewer TargetViewer { get; set; }
        //Register a DependencyProperty which has a onChange callback
        public static DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register("CurrentVerticalOffset", typeof(double), typeof(AniScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnVerticalChanged)));
        public static DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register("CurrentHorizontalOffsetOffset", typeof(double), typeof(AniScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnHorizontalChanged)));

        //When the DependencyProperty is changed change the vertical offset, thus 'animating' the scrollViewer
        private static void OnVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AniScrollViewer viewer = d as AniScrollViewer;

            viewer.TargetViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        //When the DependencyProperty is changed change the vertical offset, thus 'animating' the scrollViewer
        private static void OnHorizontalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AniScrollViewer viewer = d as AniScrollViewer;
            viewer.TargetViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }


        public double CurrentHorizontalOffset
        {
            get { return (double)this.GetValue(CurrentHorizontalOffsetProperty); }
            set { this.SetValue(CurrentHorizontalOffsetProperty, value); }
        }

        public double CurrentVerticalOffset
        {
            get { return (double)this.GetValue(CurrentVerticalOffsetProperty); }
            set { this.SetValue(CurrentVerticalOffsetProperty, value); }
        }
        Storyboard sbScroll = new Storyboard();
        DoubleAnimation ani = new DoubleAnimation();
        public void ScrollVertivalOffsetAni(double ToOffset)
        {
            if (ani != null && sbScroll.GetCurrentState() == ClockState.Active)
            {
                ani.From = ani.From + (ani.To - ani.From) / ani.Duration.TimeSpan.TotalSeconds * sbScroll.GetCurrentTime().TotalSeconds;
            }
            else
            {
                if (ani == null)
                    ani = new DoubleAnimation();
                ani.From = TargetViewer.VerticalOffset;
            }
            sbScroll.Stop();
            sbScroll.Children.Clear();
            ani.To = ToOffset;
            ani.Duration = TimeSpan.FromSeconds(0.3);
            Storyboard.SetTarget(ani, this);
            Storyboard.SetTargetProperty(ani, new PropertyPath(CurrentVerticalOffsetProperty));
            ani.Completed += new EventHandler(ani_Completed);
            sbScroll.Children.Add(ani);
            sbScroll.Begin();
        }

        void ani_Completed(object sender, EventArgs e)
        {
            if(ani!=null)
            TargetViewer.ScrollToVerticalOffset(ani.To.Value);
            ani = null;
        }
    }


}
