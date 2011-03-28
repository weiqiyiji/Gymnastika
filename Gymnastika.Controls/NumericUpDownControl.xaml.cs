using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Gymnastika.Controls
{
    /// <summary>
    /// Interaction logic for NumericUpDownControl.xaml
    /// </summary>
    public partial class NumericUpDownControl : UserControl
    {



        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); this.Value = this.Value; }
        }


        public double? MinValue
        {
            get { return (double?)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double?), typeof(NumericUpDownControl), new UIPropertyMetadata(null));


        public double? MaxValue
        {
            get { return (double?)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double?), typeof(NumericUpDownControl), new UIPropertyMetadata(null));

        
        
        // Using a DependencyProperty as the backing store for Format.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(NumericUpDownControl), new UIPropertyMetadata("{0:F2}"));


        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(UserControl),new PropertyMetadata(new PropertyChangedCallback(
                (sender,e)=>
                {
                }
                )));



        /// <summary>
        /// 越大越慢
        /// </summary>
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Speed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(NumericUpDownControl), new UIPropertyMetadata(100.0));

        
        public NumericUpDownControl()
        {
            InitializeComponent();
            mask = new Rectangle() { Fill = new SolidColorBrush(Colors.Black) };
            rootBorder.MouseLeftButtonDown += new MouseButtonEventHandler(rootBorder_MouseLeftButtonDown);
            rootBorder.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(rootBorder_ManipulationStarted);
            this.Loaded += new RoutedEventHandler(NumericUpDownControl_Loaded);
            this.LostMouseCapture += new MouseEventHandler(NumericUpDownControl_LostMouseCapture);
            this.LostTouchCapture += new EventHandler<TouchEventArgs>(NumericUpDownControl_LostTouchCapture);
            this.LostFocus += new RoutedEventHandler(NumericUpDownControl_LostFocus);
            Application.Current.MainWindow.LostFocus += new RoutedEventHandler(MainWindow_LostFocus);
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(MainWindow_PreviewMouseLeftButtonUp);
        }

        void MainWindow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }

        void MainWindow_LostFocus(object sender, RoutedEventArgs e)
        {
            ClosePopup();    
        }

        void NumericUpDownControl_LostFocus(object sender, RoutedEventArgs e)
        {
            ClosePopup();    
        }

        void NumericUpDownControl_LostTouchCapture(object sender, TouchEventArgs e)
        {
            ClosePopup();    
        }

        void NumericUpDownControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            ClosePopup();    
        }

        void NumericUpDownControl_Loaded(object sender, RoutedEventArgs e)
        {

            Value = Value;
        }

        void rootBorder_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            OpenPopup(false);
        }
        public delegate double CheckValueHandler(object sender, double NewValue);
        public event CheckValueHandler CheckValue;

        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set
            {
                if (MinValue.HasValue)
                    if (value < MinValue)
                        value = MinValue.Value;
                if (MaxValue.HasValue)
                    if (value > MaxValue)
                        value = MaxValue.Value;
                if (CheckValue != null)
                    value = CheckValue(this, value);
                lblValue.Text = string.Format(Format, value);
                this.SetValue(ValueProperty, value);
            }
        }
        private double orginValue;
        private double oy;
        void rootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenPopup(true);
            rootBorder.CaptureMouse();
        }
        Image _contentLayer = null;
        int lastTick;
        private Window FindWindow(FrameworkElement Child)
        {
            return Application.Current.MainWindow;
            //if (Child == null)
            //    return null;
            //while (Child.Parent != null)
            //{
            //    if (Child.Parent is Window)
            //        return Child.Parent as Window;
            //    Child = Child.Parent as FrameworkElement;
            //}
            //return null;

        }
        object oldContent;
        Window root;
        Canvas overlay;
        Rectangle mask;
        FrameworkElement myContent;
        ContentControl selfLayer;
        Size myContentSize;
        bool isMouseMode;
        public void OpenPopup(bool UseMouse)
        {
            root = FindWindow(this);
            isMouseMode = UseMouse;
            if (root == null)
                return;
            this.Visibility = Visibility.Collapsed;
            overlay = new Canvas();
            RenderTargetBitmap content = new RenderTargetBitmap((int)root.ActualWidth, (int)root.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            content.Render(root);
            _contentLayer = new Image() { Source = content, Width = root.ActualWidth, Height = root.ActualHeight };
            overlay.Children.Add(_contentLayer);
            overlay.Width = root.ActualWidth;
            overlay.Height = root.ActualHeight;

            mask.Opacity = 0;
            mask.Width = root.ActualWidth;
            mask.Height = root.ActualHeight;
            overlay.Children.Add(mask);

            myContent = this.Content as FrameworkElement;
            myContentSize = new Size(myContent.ActualWidth, myContent.ActualHeight);
            this.Content = null;
            selfLayer = new ContentControl() { Content = myContent };
            selfLayer.Width = myContentSize.Width; selfLayer.Height = myContentSize.Height;
            myContent.Width = myContent.Width; myContent.Height = myContent.Height;
            overlay.Children.Add(selfLayer);
            Point location = this.TranslatePoint(new Point(0, 0), root);
            selfLayer.SetValue(Canvas.LeftProperty, location.X);
            selfLayer.SetValue(Canvas.TopProperty, location.Y);
            myContent.Visibility = Visibility.Visible;
            oldContent = root.Content;
            oy = Mouse.GetPosition(root).Y;
            root.Content = overlay;
            overlay.IsEnabled = false;
            orginValue = Value;

            AnimateContent(1, 1.08, new EventHandler((sender, e) => { overlay.IsEnabled = true; }));
            //mask.MouseDown += new MouseButtonEventHandler(_contentLayer_MouseDown);
            root.SizeChanged += new SizeChangedEventHandler(root_SizeChanged);
            myContent.Effect = new DropShadowEffect() { ShadowDepth = 0 };
            lastTick = Environment.TickCount;
            if (UseMouse)
            {
                root.MouseMove += new MouseEventHandler(root_MouseMove);
                root.MouseLeftButtonUp += new MouseButtonEventHandler(root_MouseLeftButtonUp);
                root.CaptureMouse();
            }
            else
            {
                root.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(root_ManipulationDelta);
                root.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(root_ManipulationCompleted);
            }
        }

        void root_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            ClosePopup();
        }

        void root_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            double MoveSpeed = Environment.TickCount - lastTick;
            if (MoveSpeed == 0)
                MoveSpeed = 0.01;
            if (MoveSpeed > 5)
                MoveSpeed = 5;
            lastTick = Environment.TickCount;
            Value -= e.DeltaManipulation.Translation.Y / this.Speed / MoveSpeed;
        }

        void root_MouseMove(object sender, MouseEventArgs e)
        {
            double MoveSpeed = Environment.TickCount - lastTick;
            if (MoveSpeed == 0)
                MoveSpeed = 0.01;
            if (MoveSpeed > 5)
                MoveSpeed = 5;
            lastTick = Environment.TickCount;
            double newY = e.GetPosition(root).Y;
            Value -= (newY - oy) / this.Speed / MoveSpeed;
            oy = newY;

        }

        void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }


        void root_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClosePopup();
        }

        void _contentLayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }
        private void AnimateContent(double scaleFrom, double scaleTo, EventHandler Compeleted)
        {
            lblValue.RenderTransform = null;
            lblValue.RenderTransform = new ScaleTransform()
            {
                CenterY = lblValue.ActualHeight/2,
                CenterX = lblValue.ActualWidth/2
            };
           
            DoubleAnimation aniX = new DoubleAnimation();
            DoubleAnimation aniY = new DoubleAnimation();
            DoubleAnimation aniAlpha = new DoubleAnimation();
            aniAlpha.From = (scaleFrom-1)*2;
            aniAlpha.To = (scaleTo-1)*2;
            aniY.From = scaleFrom;
            aniY.To = scaleTo;
            aniX.From = scaleFrom;
            aniX.To = scaleTo;
            aniAlpha.Duration = aniY.Duration = aniX.Duration = TimeSpan.FromSeconds(0.1);

            if (Compeleted != null)
                aniY.Completed += Compeleted;

            mask.BeginAnimation(Rectangle.OpacityProperty, aniAlpha);
            lblValue.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, aniX);
            lblValue.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, aniY);


        }


        public void ClosePopup()
        {
            if (root != null && overlay != null && root.Content == overlay)
            {
                if (isMouseMode)
                {
                    root.MouseMove -= new MouseEventHandler(root_MouseMove);
                    root.MouseLeftButtonUp -= new MouseButtonEventHandler(root_MouseLeftButtonUp);
                }
                else
                {
                    root.ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(root_ManipulationDelta);
                    root.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(root_ManipulationCompleted);
                }
                overlay.IsEnabled = false;
                AnimateContent(1.08, 1, new EventHandler((sender, e) =>
                {
                    if (root == null) return;
                    root.ReleaseMouseCapture();
                    overlay.Children.Remove(mask);
                    overlay.IsEnabled = true;
                    this.Visibility = Visibility.Visible;

                    root.Content = oldContent;
                    _contentLayer = null;
                    if (selfLayer != null)
                    {
                        selfLayer.Content = null;
                        selfLayer = null;
                    }
                    myContent.Effect = null;
                    this.Content = myContent;
                }));
            }
        }


    }
}
