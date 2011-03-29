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
using System.Threading;

namespace Gymnastika.Phone.Controls
{
    public partial class Overview : UserControl
    {
        Timer updateTimer;
        public Overview()
        {
            InitializeComponent();
            Schedule.SyncCompleted += new Schedule.SyncCompeletedEventHandler(Schedule_SyncCompleted);
            updateTimer = new Timer(new TimerCallback(Update));
            updateTimer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));
        }
        void Update(object Timer)
        {
            Common.ScheduleItem item = Common.Schedule.GetNextSchduleItem();
            if (item != null)
            {
                txtSchduleName.Text = item.Name;
                txtSchduleTime.Text = item.TimeText;
                txtSchduleTime.Visibility = Visibility.Visible;
                lblTime.Visibility = Visibility.Visible;
            }
            else
            {
                txtSchduleName.Text = "没有需要执行的计划了。";
                txtSchduleTime.Visibility = Visibility.Collapsed;
                lblTime.Visibility = Visibility.Collapsed;
            }
            txtDayOfWeek.Text = Util.GetWeekDay();
        }
        void Schedule_SyncCompleted(bool Successful, int SceduleCount)
        {

            this.Dispatcher.BeginInvoke(delegate
            {
                pbSync.Visibility = Visibility.Collapsed;
                txtSync.Visibility = Visibility.Collapsed;
                if (Successful)
                    MessageBox.Show(string.Format("同步成功，你一共有{0}个计划项目。", SceduleCount), "同步成功", MessageBoxButton.OK);
                else
                    MessageBox.Show("同步失败。", "同步失败", MessageBoxButton.OK);
                btnSync.IsEnabled = true;
            });
        }
        Popup _popup;
        Canvas _overlay;
        MicroBlogPublishWindow _publishWindow;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Size size = Util.GetRootVisualSize();
            //_popup = new Popup();
            //_overlay = new Canvas();
            //_publishWindow = new MicroBlogPublishWindow();
            //Image contentLayer = new Image();
            //WriteableBitmap bmp = new WriteableBitmap((int)size.Width, (int)size.Height);
            //bmp.Render(App.Current.RootVisual, null);
            //bmp.Invalidate();
            //contentLayer.Source = bmp;
            //_overlay.Children.Add(contentLayer);
            //_overlay.Children.Add(_publishWindow);

            //_popup.Child = _overlay;
            //_publishWindow.SetValue(Canvas.WidthProperty, size.Width - 10);
            //_publishWindow.SetValue(Canvas.TopProperty, 100.0);
            //_publishWindow.SetValue(Canvas.HeightProperty, 400.0);
            //_publishWindow.SetValue(Canvas.LeftProperty, 5.0);
            //contentLayer.MouseLeftButtonDown += new MouseButtonEventHandler(contentLayer_MouseLeftButtonDown);
            //SwivelTransition st = new SwivelTransition();
            //st.Mode = SwivelTransitionMode.FullScreenIn;
            //st.GetTransition(_publishWindow).Begin();
            //_popup.IsOpen = true;
            btnSync.IsEnabled = false;
            txtSync.Visibility = Visibility.Visible;
            pbSync.Visibility = Visibility.Visible;
            Schedule.SyncPlan(UserProfile.UserProfileManager.ActiveProfile.UserId);
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
