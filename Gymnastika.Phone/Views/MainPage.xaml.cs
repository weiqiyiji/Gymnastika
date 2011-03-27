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
using Microsoft.Phone.Controls;
using Gymnastika.Phone.Common;
namespace Gymnastika.Phone.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        SchduleListener listener = new SchduleListener();
        public MainPage()
        {
            Common.DefualtTransition.SetNavigationTransition(this);
            InitializeComponent();
            schduleList.SchduleItems.Add(
                new Common.ScheduleItem()
                {
                    Time = DateTime.Now+TimeSpan.FromSeconds(30),
                    Duration = TimeSpan.FromSeconds(30),
                    Name = "早餐",
                    Point = 7,
                    Calorie = 80,
                });
            schduleList.SchduleItems.Add(
                    new Common.ScheduleItem()
                    {
                        Time =DateTime.Now+TimeSpan.FromSeconds(100),
                        Duration = TimeSpan.FromMinutes(1),
                        Name = "走路去教室",
                        Point = 7,
                        Calorie = -34,
                    });
            schduleList.SchduleItems.Add(
        new Common.ScheduleItem()
        {
            Time =DateTime.Now+TimeSpan.FromMinutes(1),
            Duration = TimeSpan.FromSeconds(60),
            Name = "坐着上课",
            Point = 10,
            Calorie = -20,
        });

            schduleList.SchduleItems.Add(
            new Common.ScheduleItem()
            {
                Time = DateTime.Now+TimeSpan.FromMinutes(2),
                Duration = TimeSpan.FromSeconds(5),
                Name = "睡着上课",
                Point = 5,
                Calorie = -10,
            });
            schduleList.SchduleItems.Add(
                new Common.ScheduleItem()
                {
                    Time = DateTime.Now +TimeSpan.FromMinutes(3),
                    Duration = TimeSpan.FromMinutes(2),
                    Name = "午餐",
                    Point = 5,
                    Calorie = 170,
                });
            schduleList.Sort();
            foreach (ScheduleItem item in schduleList.SchduleItems)
            {
                listener.Children.Add(item);
            }
            listener.ScheduleBegin += new EventHandler<SchduleListener.ScheduleArg>(listener_ScheduleBegin);
            listener.ScheduleCompelted += new EventHandler<SchduleListener.ScheduleArg>(listener_ScheduleCompelted);
            listener.ScheduleStatusChagned += new EventHandler<SchduleListener.ScheduleStatusChangedArg>(listener_ScheduleStatusChagned);
            listener.Start(this.Dispatcher);
          
        }

        void listener_ScheduleStatusChagned(object sender, SchduleListener.ScheduleStatusChangedArg e)
        {
            MessageBox.Show(string.Format("{0} status changed from {1} to {2}.", e.Item.Name, e.OldStatus, e.NewStatus));
        }

        void listener_ScheduleCompelted(object sender, SchduleListener.ScheduleArg e)
        {
            MessageBox.Show(string.Format("{0} compeleted.", e.Item.Name));
            e.Item.Status = ScheduleItemStatus.Done;
        }

        void listener_ScheduleBegin(object sender, SchduleListener.ScheduleArg e)
        {
            if (MessageBox.Show(string.Format("Begin {0}?", e.Item.Name), "Begin new schdule", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                e.Item.Status = ScheduleItemStatus.Active;
                e.Item.Time = DateTime.Now;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            scoreViewer.ScoreItems.Clear();
            foreach (ScheduleItem item in schduleList.SchduleItems)
            {
                scoreViewer.ScoreItems.Add(item);
            }
            scoreViewer.FinishChange();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            scoreViewer.ScoreItems.Clear();
            scoreViewer.FinishChange();
        }
    }
}