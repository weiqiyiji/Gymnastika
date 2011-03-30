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
using System.Xml.Linq;
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
            listener.ScheduleCompelted += new EventHandler<SchduleListener.ScheduleArg>(listener_ScheduleCompelted);
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
            //ParseNotification("<schedule id=\"15\">" +
            //                  "<connection id=\"3\"/>" +
            //                  "<user id=\"1\" />"
            //                  + "<data>"
            //                  + "<SportsPlanTaskItem xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">"
            //                 + "<Calories>300</Calories>"
            //                 + "<Duration>30</Duration>"
            //                 + "<Id>21</Id>"
            //                 + "<Minutes>60</Minutes>"
            //                 + "<Score>40.72063178677196</Score>"
            //                 + "<SportName>桌球</SportName>"
            //                 + "<Time>2011-03-29T17:53:00</Time>"
            //                 + "</SportsPlanTaskItem>"
            //                 + "</data>"
            //                 + "</schedule>");
        }
        void ParseNotification(string xml)
        {
            this.Dispatcher.BeginInvoke(delegate
            {
                XDocument doc = XDocument.Parse(xml);
                int id = 0;
                foreach (XElement element in doc.Elements())
                {

                    if (element.Name.LocalName == "schedule")
                    {
                        foreach (XAttribute attr in element.Attributes())
                        {
                            if (attr.Name.LocalName == "id")
                                id = int.Parse(attr.Value);
                        }
                        foreach (XElement element2 in element.Elements())
                        {
                            if (element2.Name.LocalName == "data")
                            {
                                Common.ScheduleItem item = Schedule.GetItemFromXml(id, element2.FirstNode.ToString());
                                OnNotificationItem(item);
                            }
                        }
                    }
                }
            });

        }
        void OnNotificationItem(Common.ScheduleItem Item)
        {
            bool flag = false;
            foreach (var i in Schedule.GetAllSchedules())
            {
                if (Item.ID == i.ID)
                {
                    flag = true;
                    Item = i;
                    break;
                }
            }
            if (!flag)
                Schedule.AddItem(Item);
            if (Item.Status != ScheduleItemStatus.Active && Item.Status != ScheduleItemStatus.Done)
            {
                if (MessageBox.Show(string.Format("开始{0}?", Item.Name), "提示", MessageBoxButton.OKCancel)==MessageBoxResult.OK)
                {
                    Item.Status = ScheduleItemStatus.Active;
                } 
            }
        }
        void pushNotificationService_HttpNotificationReceived(object sender, Microsoft.Phone.Notification.HttpNotificationEventArgs e)
        {
            using (StreamReader reader = new StreamReader(e.Notification.Body, Encoding.UTF8))
            {
                ParseNotification(reader.ReadToEnd());
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
            PlanSync.CompeleteTask(e.Item.ID, new PlanSync.CompeleteTaskCallback((id, successful) =>
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