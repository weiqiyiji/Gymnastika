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
        public MainPage()
        {
            Common.DefualtTransition.SetNavigationTransition(this);
            InitializeComponent();
            schduleList.SchduleItems.Add(
                new Common.ScheduleItem()
                {
                    Time = new DateTime(2011, 3, 24, 7, 3, 4),
                    Duration = TimeSpan.FromMinutes(30),
                    Name = "早餐",
                    Point = 7,
                    Calorie = 20,
                    Status = ScheduleItemStatus.Done
                });
            schduleList.SchduleItems.Add(
                    new Common.ScheduleItem()
                    {
                        Time = new DateTime(2011, 3, 24, 8, 20, 0),
                        Duration = TimeSpan.FromMinutes(15),
                        Name = "走路去教室",
                        Point = 7,
                        Calorie = -34,
                        Status = ScheduleItemStatus.Done
                    });
            schduleList.SchduleItems.Add(
        new Common.ScheduleItem()
        {
            Time = new DateTime(2011, 3, 24, 8, 30, 0),
            Duration = TimeSpan.FromMinutes(45),
            Name = "坐着上课",
            Point = 10,
            Calorie = -20,
            Status = ScheduleItemStatus.Done
        });

            schduleList.SchduleItems.Add(
            new Common.ScheduleItem()
            {
                Time = new DateTime(2011, 3, 24, 9, 25, 0),
                Duration = TimeSpan.FromMinutes(45),
                Name = "睡着上课",
                Point = 5,
                Calorie = -10,
                Status = ScheduleItemStatus.Active
            });
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