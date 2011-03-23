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

namespace Gymnastika.Phone.Controls
{
    public partial class UserSelectorItem : UserControl
    {
        public UserProfile.Profile Profile { get; set; }
        private bool m_Expand = false;
        public delegate void ExpandChagnedHandler(object sender, bool value);
        public event EventHandler<EventArgs> SiginClick;
        public event EventHandler<EventArgs> DeleteClick;
        public event ExpandChagnedHandler ExpandChanged;
        public ImageSource UserIcon
        {
            get { return UserIconImage.Source; }
            set { UserIconImage.Source = value; }
        }
        public string Username
        {
            get { return tbUsername.Text; }
            set { tbUsername.Text = value; }
        }
        public bool Expaned { get { return m_Expand; } set { SetExpand(value); } }
        SolidColorBrush sbBackgroundBakup;
        public UserSelectorItem()
        {
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(UserSelectorItem_ManipulationCompleted);
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(UserSelectorItem_ManipulationStarted);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(UserSelectorItem_MouseLeftButtonUp);
            InitializeComponent();
    
            this.Loaded += new RoutedEventHandler(UserSelectorItem_Loaded);

        }

        void UserSelectorItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = GetHeight(false);
            sbBackgroundBakup = LayoutRoot.Background as SolidColorBrush;
            SetExpand(false);
        }
        public double GetHeight(bool isExpanded)
        {
            if (!isExpanded)
            {
                return hlDelete.Margin.Top  +60 + LayoutRoot.BorderThickness.Top + LayoutRoot.BorderThickness.Bottom+this.Margin.Top+this.Margin.Bottom;
            }
            else
            {
                return hlDelete.Margin.Top + hlDelete.ActualHeight + 60 + LayoutRoot.BorderThickness.Top + LayoutRoot.BorderThickness.Bottom+this.Margin.Top+this.Margin.Bottom;
            }
        }
        public void SetExpand(bool value)
        {
            if (m_Expand == value) return;
            m_Expand = value;
            double toHeight;
            if (value)
            {
                this.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                toHeight = GetHeight(true);
                if (Parent is StackPanel)
                {
                    double topOffset = 0;
                    StackPanel sp = Parent as StackPanel;
                    foreach (UIElement el in sp.Children)
                    {
                        if (el is UserSelectorItem)
                        {
                            if (!el.Equals(this))
                            {
                                (el as UserSelectorItem).SetExpand(false);
                                topOffset += GetHeight(false);
                            }
                            else
                            {
                                if (sp.Parent is ScrollViewer)
                                {

                                    ScrollViewer sv = sp.Parent as ScrollViewer;
                                    AniScrollViewer ani = AniScrollViewer.SetAni(sv);
                                    ani.ScrollVertivalOffsetAni(topOffset - sv.ActualHeight / 2 + toHeight / 2);

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.Background = null;
                toHeight = GetHeight(false);
            }
            if (ExpandChanged != null)
                ExpandChanged(this, value);
            DoubleAnimation aniH = new DoubleAnimation();
            aniH.From = this.Height;
            aniH.To = toHeight;
            aniH.Duration = TimeSpan.FromSeconds(0.3);
            Storyboard.SetTarget(aniH, this);
            Storyboard.SetTargetProperty(aniH, new PropertyPath(Control.HeightProperty));
            Storyboard sbH = new Storyboard();
            sbH.Children.Add(aniH);
            sbH.Begin();
        }
        void UserSelectorItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetExpand(true);
            this.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
        }

        void UserSelectorItem_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (!this.m_Expand)
                LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
        }

        void UserSelectorItem_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (!this.m_Expand)
                LayoutRoot.Background = sbBackgroundBakup;
        }

        private void hlSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (this.SiginClick != null)
                SiginClick(this, new EventArgs());

        }

        private void hlDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.DeleteClick != null)
                this.DeleteClick(this, new EventArgs());
        }
    }
}
