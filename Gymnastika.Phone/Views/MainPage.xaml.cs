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
namespace Gymnastika.Phone.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        PushNotificationService pushNotificationService = new PushNotificationService();
        PlanSync sync = new PlanSync();
        SchduleListener listener = new SchduleListener();
        Timer UpdateSchduleTimer;
        public MainPage()
        {
            InitializeComponent();
            Common.DefualtTransition.SetNavigationTransition(this);
        }
        void UpdateSchdule(object timer)
        {
            Common.Schedule.SyncPlan(UserProfile.UserProfileManager.ActiveProfile.UserId);
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

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            UpdateSchdule(null);
        }
    }
}