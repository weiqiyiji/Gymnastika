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
using Gymnastika.Phone.PushNotification;
using Gymnastika.Phone.Sync;
using System.Threading;
using System.IO;
using System.Text;
namespace Gymnastika.Phone.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
      
        PlanSync sync = new PlanSync();
        SchduleListener listener = new SchduleListener();
        Timer UpdateSchduleTimer;
        public MainPage()
        {
            InitializeComponent();
            Common.DefualtTransition.SetNavigationTransition(this);
            Util.pushNotificationService.SubscribeCompleted += new EventHandler<SubscribeCompletedEventArgs>(pushNotificationService_SubscribeCompleted);
            Util.pushNotificationService.HttpNotificationReceived += new EventHandler<Microsoft.Phone.Notification.HttpNotificationEventArgs>(pushNotificationService_HttpNotificationReceived);
           listener.ScheduleCompelted+=new EventHandler<SchduleListener.ScheduleArg>(listener_ScheduleCompelted);
            Common.Schedule.ItemAdded += new EventHandler<Schedule.SchduleEventArgs>(Schedule_ItemAdded);
            Common.Schedule.ItemRemoved += new EventHandler<Schedule.SchduleEventArgs>(Schedule_ItemRemoved);
            Common.Schedule.ItemsCleared += new EventHandler<Schedule.SchduleEventArgs>(Schedule_ItemsCleared);
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(MainPage_BackKeyPress);
        }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UserProfile.UserProfileManager.ActiveProfile = null;
            
        }

        void Schedule_ItemsCleared(object sender, Schedule.SchduleEventArgs e)
        {
            listener.Children.Clear();
        }

        void Schedule_ItemRemoved(object sender, Schedule.SchduleEventArgs e)
        {
            listener.Children.Remove(e.Item);
        }

        void Schedule_ItemAdded(object sender, Schedule.SchduleEventArgs e)
        {
            listener.Children.Add(e.Item);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Util.pushNotificationService.Connect();
        }

        void pushNotificationService_HttpNotificationReceived(object sender, Microsoft.Phone.Notification.HttpNotificationEventArgs e)
        {
            using (StreamReader reader = new StreamReader(e.Notification.Body, Encoding.UTF8))
            {
                List<ScheduleItem> items = Schedule.ParseSchduleItems(reader.ReadToEnd());
                foreach (ScheduleItem item in items)
                {
                    ScheduleItem savedItem = Schedule.GetScheduleItemById(item.ID);
                    if (savedItem != null && savedItem.Status != ScheduleItemStatus.Done)
                    {
                        if (MessageBox.Show(string.Format("开始 {0} ?", item.Name), "提醒", MessageBoxButton.OK) ==MessageBoxResult.OK)
                        {
                            savedItem.Status = ScheduleItemStatus.Active;
                        }
                    }
                }
            }
        }

        void pushNotificationService_SubscribeCompleted(object sender, SubscribeCompletedEventArgs e)
        {
            
        }
        void UpdateSchdule(object timer)
        {
            Common.Schedule.SyncPlan(UserProfile.UserProfileManager.ActiveProfile.UserId);
        }
        void listener_ScheduleCompelted(object sender, SchduleListener.ScheduleArg e)
        {
         PlanSync.CompeleteTask(e.Item.ID,new PlanSync.CompeleteTaskCallback((id,successful)=>
         {
             
         }));  
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

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            UpdateSchdule(null);
        }
    }
}