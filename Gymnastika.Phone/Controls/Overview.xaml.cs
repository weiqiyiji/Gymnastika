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
using System.Windows.Controls.Primitives;
using Gymnastika.Phone.Common;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace Gymnastika.Phone.Controls
{
    public partial class Overview : UserControl
    {
        public Overview()
        {
            InitializeComponent();
        }
        Popup _popup;
        Canvas _overlay;
        MicroBlogPublishWindow _publishWindow;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Size size = Util.GetRootVisualSize();
            _popup = new Popup();
            _overlay = new Canvas();
            _publishWindow = new MicroBlogPublishWindow();
            Image contentLayer = new Image();
            WriteableBitmap bmp = new WriteableBitmap((int)size.Width, (int)size.Height);
            bmp.Render(App.Current.RootVisual, null);
            bmp.Invalidate();
            contentLayer.Source = bmp;
            _overlay.Children.Add(contentLayer);
            _overlay.Children.Add(_publishWindow);
            
            _popup.Child = _overlay;
            _publishWindow.SetValue(Canvas.WidthProperty, size.Width - 10);
            _publishWindow.SetValue(Canvas.TopProperty, 100.0);
            _publishWindow.SetValue(Canvas.HeightProperty, 400.0);
            _publishWindow.SetValue(Canvas.LeftProperty, 5.0);
            contentLayer.MouseLeftButtonDown += new MouseButtonEventHandler(contentLayer_MouseLeftButtonDown);
            SwivelTransition st = new SwivelTransition();
            st.Mode = SwivelTransitionMode.FullScreenIn;
            st.GetTransition(_publishWindow).Begin();
            _popup.IsOpen = true;
            
        }
        void ClosePopup()
        {
            SwivelTransition st = new SwivelTransition();
            st.Mode = SwivelTransitionMode.FullScreenOut;
            var it = st.GetTransition(_publishWindow);
            it.Completed += new EventHandler(it_Completed);
            it.Begin();
        }

        void it_Completed(object sender, EventArgs e)
        {
            if (_popup != null)
                _popup.IsOpen = false;
        }
        void contentLayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }
    }
}
