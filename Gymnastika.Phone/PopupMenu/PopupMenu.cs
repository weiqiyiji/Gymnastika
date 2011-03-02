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
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
namespace Gymnastika.Phone.PopupMenu
{
    public class PopupMenu : Control
    {
        PhoneApplicationFrame _rootVisual;
        Canvas _overlay;
        FrameworkElement _owner;
        double width, height;
        public string Title { get; set; }
        public PopupMenu(FrameworkElement Owner)
        {
            InitalizeRootVisual();
            _owner = Owner;
            Owner.SizeChanged += new SizeChangedEventHandler(PopupMenu_SizeChanged);
            FrameworkElement fe = Owner;
            while (fe != null)
            {
                if (fe is PhoneApplicationPage)
                {
                    (fe as PhoneApplicationPage).BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(PopupMenu_BackKeyPress);
                }
                fe = fe.Parent as FrameworkElement;
            }
        }

        void PopupMenu_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_Popup != null && _Popup.IsOpen)
            {
                e.Cancel = true;
                Close();
            }
    
        }
        private void InitalizeRootVisual()
        {
            if (null == _rootVisual)
            {
                _rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
                width = _rootVisual.ActualWidth;
                height = _rootVisual.ActualHeight;
                bool portrait = PageOrientation.Portrait == (PageOrientation.Portrait & _rootVisual.Orientation);
                width = portrait ? _rootVisual.ActualWidth : _rootVisual.ActualHeight;
                height = portrait ? _rootVisual.ActualHeight : _rootVisual.ActualWidth;
            }
        }
        Popup _Popup;
        Image contentLayer;
        PopupMenuLayer menuLayer;
        private List<MenuItem> m_Items = new List<MenuItem>();
        public List<MenuItem> Items { get { return m_Items; } }

        public delegate void MenuClickHandler(object sender, int MenuID, MenuItem Menu);
        public event MenuClickHandler MenuClick;
        public void Open()
        {
            
            InitalizeRootVisual();
            _overlay = new Canvas() { Background = new SolidColorBrush(Colors.Transparent) };
            _overlay.Children.Add(this);
            _overlay.Width = width;
            _overlay.Height = height;
            WriteableBitmap orgin = new WriteableBitmap((int)width,(int)height);
            orgin.Render(Application.Current.RootVisual, null);
            orgin.Invalidate();
            UIElement backgroundLayer = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = (Brush)Application.Current.Resources["PhoneBackgroundBrush"]
            };
            _overlay.Children.Insert(0, backgroundLayer);
            contentLayer = new Image()
             {
                 Source = orgin,
             };

            _overlay.Children.Insert(1, contentLayer);
            Point point = SafeTransformToVisual(_owner, _rootVisual).Transform(new Point());

            _overlay.Children.Add(new Button());
            _overlay.MouseLeftButtonDown += new MouseButtonEventHandler(_overlay_MouseLeftButtonDown);


            TransformGroup transforms = new TransformGroup();
            if (null != _rootVisual)
            {
                switch (_rootVisual.Orientation)
                {
                    case PageOrientation.LandscapeLeft:
                        transforms.Children.Add(new RotateTransform { Angle = 90 });
                        transforms.Children.Add(new TranslateTransform { X = _rootVisual.ActualWidth });
                        break;
                    case PageOrientation.LandscapeRight:
                        transforms.Children.Add(new RotateTransform { Angle = -90 });
                        transforms.Children.Add(new TranslateTransform { Y = _rootVisual.ActualHeight });
                        break;
                }
            }
            _overlay.RenderTransform = transforms;
            _rootVisual.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(_rootVisual_OrientationChanged);
            menuLayer = new PopupMenuLayer();
            menuLayer.SizeChanged += new SizeChangedEventHandler(menuLayer_SizeChanged);
            for (int i = 0; i < Items.Count; i++)
            {
                menuLayer.AddMenu(i, Items[i].Icon, Items[i].Text);
            }
            _overlay.Children.Add(menuLayer);
            menuLayer.Title = this.Title;
            menuLayer.Visibility = Visibility.Collapsed;
            menuLayer.MenuClick += new PopupMenuLayer.MenuClickHandler(menuLayer_MenuClick);
            _Popup = new Popup() { Child = _overlay };

            SizeChanged += new SizeChangedEventHandler(PopupMenu_SizeChanged);
            _Popup.IsOpen = true;

            double from = 1;
            double to = 0.96;
            ScaleTransform stf = new ScaleTransform();
            stf.CenterX = width / 2;
            stf.CenterY = height / 2;
            contentLayer.RenderTransform = stf;

            DoubleAnimation aniX = new DoubleAnimation();
            Storyboard.SetTarget(aniX, stf);
            Storyboard.SetTargetProperty(aniX, new PropertyPath(ScaleTransform.ScaleXProperty));
            DoubleAnimation aniY = new DoubleAnimation();
            Storyboard.SetTarget(aniY, stf);
            Storyboard.SetTargetProperty(aniY, new PropertyPath(ScaleTransform.ScaleYProperty));

            aniX.From = from;
            aniX.To = to;
            aniY.From = from;
            aniY.To = to;
            aniX.Duration = TimeSpan.FromSeconds(0.5);
            aniY.Duration = TimeSpan.FromSeconds(0.5);
            Storyboard sb = new Storyboard();
            sb.Children.Add(aniY);
            sb.Children.Add(aniX);
            sb.Begin();
            Focus();
        }

        void _rootVisual_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            Close();
        }

        void menuLayer_MenuClick(object sender, int MenuID)
        {
            if (MenuClick != null)
                MenuClick(this, MenuID, Items[MenuID]);
            Close();
        }

        void menuLayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        public void Close()
        {
            if (contentLayer == null) return;
            menuLayer.Hide();
            ScaleTransform stf = contentLayer.RenderTransform as ScaleTransform;
            double from = stf.ScaleX;
            double to = 1;

            stf.CenterX = width / 2;
            stf.CenterY = height / 2;
            contentLayer.RenderTransform = stf;
            DoubleAnimation aniX = new DoubleAnimation();
            Storyboard.SetTarget(aniX, stf);
            Storyboard.SetTargetProperty(aniX, new PropertyPath(ScaleTransform.ScaleXProperty));
            DoubleAnimation aniY = new DoubleAnimation();
            Storyboard.SetTarget(aniY, stf);
            Storyboard.SetTargetProperty(aniY, new PropertyPath(ScaleTransform.ScaleYProperty));

            aniX.From = from;
            aniX.To = to;
            aniY.From = from;
            aniY.To = to;
            aniX.Duration = TimeSpan.FromSeconds(0.1);
            aniY.Duration = TimeSpan.FromSeconds(0.1);
            Storyboard sb = new Storyboard();
            sb.Children.Add(aniY);
            sb.Children.Add(aniX);
            sb.Begin();
            sb.Completed += new EventHandler(sb_Completed);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            if (_Popup != null)
            {
                _Popup.IsOpen = false;
                _Popup = null;
                contentLayer = null;
            }
            menuLayer = null;
        }
        void _overlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == this || e.OriginalSource == contentLayer)
                Close();
        }
        private static GeneralTransform SafeTransformToVisual(UIElement element, UIElement visual)
        {
            GeneralTransform result;
            try
            {
                result = element.TransformToVisual(visual);
            }
            catch (ArgumentException)
            {
                // Not perfect, but better than throwing an exception
                result = new TranslateTransform();
            }
            return result;
        }
        void PopupMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //  width = _owner.ActualWidth;
            // height = _owner.ActualHeight;
        }
    }
}
