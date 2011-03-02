using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gymnastika.Phone.Common;
using Microsoft.Phone.Controls;
namespace Gymnastika.Phone.Controls
{
    public partial class SchduleListItem : UserControl
    {

        public static DependencyProperty SelectedProperty = DependencyProperty.Register(
            "SelectedProperty", typeof(bool), typeof(SchduleListItem), new PropertyMetadata(
                new PropertyChangedCallback((obj, prop) =>
                    {
                        if (prop.NewValue.Equals(true))
                        {
                            if (obj is SchduleListItem)
                            {
                                SchduleListItem item = obj as SchduleListItem;
                                if (item.Parent is Panel)
                                {
                                    Panel parent = item.Parent as Panel;
                                    foreach (Control ctl in parent.Children)
                                    {
                                        if (ctl is SchduleListItem && !ctl.Equals(item))
                                        {
                                            ((SchduleListItem)ctl).Selected = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    )));
        public bool Selected
        {
            get
            {
                return (bool)this.GetValue(SelectedProperty);
            }
            set
            {
                if (Selected != value)
                {
                    this.SetValue(SelectedProperty, value);
                    UpdateContent();
                }
            }
        }
        public ScheduleItem Item
        {
            get { return m_Item; }
            set { m_Item = value; UpdateContent(); }
        }
        private ScheduleItem m_Item;
        GestureListener gestureListener;
        public int Index { get; set; }
        Storyboard HeightStoryboard = new Storyboard();
        DoubleAnimation HeightAnimation = new DoubleAnimation();
        private void AnimateHeight(double targetHeight)
        {
            HeightStoryboard.Stop();
            if (this.ActualHeight == targetHeight)
            {
                return;
            }
            double from;
            HeightStoryboard.Children.Clear();
            from = this.ActualHeight;
        
            HeightAnimation.From = from;
            HeightAnimation.To = targetHeight;
            HeightAnimation.Duration = TimeSpan.FromSeconds(0.15);
            Storyboard.SetTarget(HeightAnimation, this);
            Storyboard.SetTargetProperty(HeightAnimation, new PropertyPath(HeightProperty));
            HeightStoryboard.Children.Add(HeightAnimation);
            HeightStoryboard.Begin();
        }
        private void UpdateContent()
        {
            double extraHeight = borderDetails.Margin.Left + borderRoot.Margin.Top + borderRoot.Margin.Bottom + borderRoot.BorderThickness.Bottom + borderRoot.BorderThickness.Top;
            if (Selected)
            {
                AnimateHeight(borderDetails.Margin.Top + borderDetails.Height + extraHeight);
            }
            else
            {
                AnimateHeight(txtStatus.Height + txtStatus.Margin.Top + 8);
            }
        }
        public SchduleListItem()
        {
            InitializeComponent();
            Selected = false;
            UpdateContent();
            gestureListener = GestureService.GetGestureListener(this);
            gestureListener.Tap += new EventHandler<GestureEventArgs>(gestureListener_Tap);
        }

        void gestureListener_Tap(object sender, GestureEventArgs e)
        {
            this.Selected = true;
        }
        public SchduleListItem(int Index, ScheduleItem Item)
            : this()
        {
            this.Index = Index;
            m_Item = Item;
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(SchduleListItem_ManipulationStarted);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(SchduleListItem_ManipulationCompleted);

        }

        void SchduleListItem_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            // this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
        }

        void SchduleListItem_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            // this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
