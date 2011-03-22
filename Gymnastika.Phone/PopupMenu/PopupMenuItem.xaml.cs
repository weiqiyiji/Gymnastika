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

namespace Gymnastika.Phone.PopupMenu
{
    public partial class PopupMenuItem : UserControl
    {
        public delegate void ClickHandler(object sender, MouseButtonEventArgs e);
        public event ClickHandler Click;
        public double MenuItemHeight { get; private set; }
        PlaneProjection plane = new PlaneProjection();
        public PopupMenuItem()
        {

            InitializeComponent();
            LayoutRoot.Background = new SolidColorBrush(Colors.Transparent);
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(PopupMenuItem_ManipulationStarted);
            this.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(PopupMenuItem_ManipulationDelta);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(PopupMenuItem_ManipulationCompleted);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(PopupMenuItem_MouseLeftButtonUp);
            MenuItemHeight = LayoutRoot.Height;
            line1.Loaded += new RoutedEventHandler(line1_Loaded);
        }

        void line1_Loaded(object sender, RoutedEventArgs e)
        {
            line1.X1 = 0;
            line1.X2 = line1.ActualWidth;
            line1.Y1 = line1.Y2 = line1.ActualHeight - line1.StrokeThickness ;
        }

        void PopupMenuItem_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (plane != null)
            {
                plane.RotationY = 15 - (this.TransformToVisual(this).Transform(e.ManipulationOrigin).X) /
                     (App.Current.RootVisual as FrameworkElement).ActualWidth * 30;

            }

        }
        internal int ID { get; set; }
        void PopupMenuItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }
        public ImageSource Image
        {
            get
            { return image1.Source; }
            set
            {
                image1.Source = value;
                if (value != null)
                {
                    image1.Visibility = Visibility.Visible;
                    textBlock1.Margin = new Thickness(image1.ActualWidth + 3, 0, 0, 0);
                }
                else
                {
                    image1.Visibility = Visibility.Collapsed;
                    textBlock1.Margin = new Thickness(3, 0, 0, 0);
                }

            }
        }
        public string Text { get { return textBlock1.Text; } set { textBlock1.Text = value; } }
        void PopupMenuItem_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            AnimateBackground(Colors.Transparent);
        }

        ColorAnimation BackgoroundAnimation = new ColorAnimation();
        Storyboard BackgroundStoryBoard = new Storyboard();
        void AnimateBackground(Color TargetColor)
        {
            BackgroundStoryBoard.Stop();
            BackgoroundAnimation.From = (LayoutRoot.Background as SolidColorBrush).Color;
            BackgoroundAnimation.To = TargetColor;
            BackgoroundAnimation.Duration = TimeSpan.FromSeconds(0.2);
            Storyboard.SetTarget(BackgoroundAnimation, LayoutRoot.Background);
            Storyboard.SetTargetProperty(BackgoroundAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            BackgroundStoryBoard.Children.Clear();
            BackgroundStoryBoard.Children.Add(BackgoroundAnimation);
            BackgroundStoryBoard.Begin();
            BackgroundStoryBoard.Completed += new EventHandler(BackgroundStoryBoard_Completed);
        }

        void BackgroundStoryBoard_Completed(object sender, EventArgs e)
        {
            plane.RotationY = 0;
            BackgroundStoryBoard.Stop();
            BackgroundStoryBoard.Children.Clear();
            (LayoutRoot.Background as SolidColorBrush).Color = (Color)BackgoroundAnimation.To;

        }
        void PopupMenuItem_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            this.Projection = plane;
            AnimateBackground(Colors.LightGray);
        }

    }
}
