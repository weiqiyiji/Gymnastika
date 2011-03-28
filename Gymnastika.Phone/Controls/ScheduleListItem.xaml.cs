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

        #region DependencyProperties
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
        #endregion
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
                if (value)
                {
                    if (Parent is Panel)
                    {
                        Panel p = Parent as Panel;
                        if (p.Parent is ScrollViewer)
                        {
                            AniScrollViewer viewer = AniScrollViewer.SetAni(p.Parent as ScrollViewer);
                            double topOffset = 0;
                            for (int i=0;i<p.Children.Count;++i)
                            {
                                FrameworkElement fe = p.Children[i] as FrameworkElement;
                                if (fe.Equals(this))
                                    break;
                                topOffset += GetHeight(false);
                            }
                            viewer.ScrollVertivalOffsetAni(topOffset);
                        }
                    }
                }
            }
        }
        private EventHandler ItemContentChanged;
        public ScheduleItem Schedule
        {
            get { return m_Item; }
            set {
                if (m_Item != value)
                {
                    value.ScheduleContentChagned += ItemContentChanged;
                    if (m_Item != null)
                    {
                        m_Item.ScheduleContentChagned -= ItemContentChanged;
                    }
                    m_Item = value;
                    UpdateContent();
                }
            }
        }

        void ScheduleContentChagned(object sender, EventArgs e)
        {
            UpdateContent();
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
        private double GetHeight(bool Selected)
        {
            double extraHeight = DetailBorder.Margin.Left + borderRoot.Margin.Top + borderRoot.Margin.Bottom + borderRoot.BorderThickness.Bottom + borderRoot.BorderThickness.Top;
            if (Selected)
            {
                return DetailBorder.Margin.Top + DetailBorder.Height + extraHeight;
            }
            else
            {
                return txtStatus.Height + txtStatus.Margin.Top + 8;
            }
        }
        private void UpdateContent()
        {
            double extraHeight = DetailBorder.Margin.Left + borderRoot.Margin.Top + borderRoot.Margin.Bottom + borderRoot.BorderThickness.Bottom + borderRoot.BorderThickness.Top;
            if (Selected)
            {
                AnimateHeight(DetailBorder.Margin.Top + DetailBorder.Height + extraHeight);
            }
            else
            {
                AnimateHeight(txtStatus.Height + txtStatus.Margin.Top + 8);
            }
            if (Schedule != null)
            {
                txtStatus.Text = Schedule.StatusText;
                if (Schedule.Duration.HasTimeSpan && Schedule.Duration.TimeSpan.TotalSeconds > 0)
                    txtTime.Text = string.Format("{0} - {1}",
                        Schedule.Time.ToString("HH:mm:ss"),
                        Schedule.Time.Add(Schedule.Duration.TimeSpan).ToString("HH:mm:ss"));
                else
                    txtTime.Text = Schedule.Time.ToString("HH:mm:ss");
                txtName.Text = Schedule.Name;

                
                string resName = "Bg" + Enum.GetName(typeof(ScheduleItemStatus), Schedule.Status);
                if (Resources.Contains(resName)&&Resources[resName] is Brush)
                {

                    borderRoot.Background = Resources[resName] as Brush;
                }
                else
                    borderRoot.Background = Resources["BgNormal"] as Brush;
                if (Schedule.Calorie > 0)
                {
                    txtCalorie.Text = string.Format("卡路里摄入：{0} 大卡", Schedule.Calorie);
                    txtCalorie.Foreground = Resources["FgIn"] as Brush;
 
                }
                else if (Schedule.Calorie < 0)
                {
                    txtCalorie.Text = string.Format("卡路里消耗：{0} 大卡", -Schedule.Calorie);
                    txtCalorie.Foreground = Resources["FgOut"] as Brush;
                }
                txtCalorie.Visibility = Schedule.Calorie != 0 ? Visibility.Visible : Visibility.Collapsed;
            }

           
        }
        public SchduleListItem()
        {
            InitializeComponent();
            ItemContentChanged = new EventHandler(ScheduleContentChagned);
            Selected = false;
            UpdateContent();
            gestureListener = GestureService.GetGestureListener(this);
            gestureListener.Tap += new EventHandler<GestureEventArgs>(gestureListener_Tap);
            
        }

        void gestureListener_Tap(object sender, GestureEventArgs e)
        {
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            bool flag = false;
            while (fe != null)
            {
                if (fe.Equals(borderRoot))
                {
                    flag = true;
                    break;
                }
                else if (fe.Equals(DetailBorder))
                {
                    flag = false;
                    break;
                }
                fe = fe.Parent as FrameworkElement;
            }
            if (!flag)
                return;
            Rect rc = new Rect(0, 0, DetailBorder.RenderSize.Width, DetailBorder.RenderSize.Height);
            if (rc.Contains(e.GetPosition(DetailBorder)))
                return;
            this.Selected = !this.Selected;
           
        }
        public SchduleListItem(int Index, ScheduleItem Item)
            : this()
        {
            this.Index = Index;
            this.Schedule = Item;
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