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
        private bool IsScanning = false;
        private FoodInfo CurrentFood;
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
            barcodeSanner1.ScanBegin += new EventHandler<Controls.BarcodeScanBeginArgs>(barcodeSanner1_ScanBegin);
            barcodeSanner1.ScanCompeleted += new EventHandler<Controls.BarcodeScanCompeletedArgs>(barcodeSanner1_ScanCompeleted);

        }
        bool CanTakeFood(double calory)
        {
            double sum = 0;
            double caloryNeed = 150;
            foreach (var t in Schedule.GetAllSchedules())
            {
                if (t.Calorie > 0)
                    sum += t.Calorie;
            }
            return sum + calory < caloryNeed;
        }
        void barcodeSanner1_ScanCompeleted(object sender, Controls.BarcodeScanCompeletedArgs e)
        {
            IsScanning = false;
            if (!e.Successful)
            {
                barcodeSanner1.TestScan();
                return;
            }
            else
            {
                FoodLibrary lib = new FoodLibrary(new Uri("http://localhost:998"));
                lib.GetFoodInfoCompeleted += new EventHandler<GetFoodInfoCompeletedArgs>((s, arg) =>
                {
                    if (arg.Barcode == e.Code)
                    {
                        CurrentFood = arg.Info;
                        if (arg.Info == null)
                        {
                            txtInfo.Text = "没有找到该食物。";
                            txtSuggest.Text = "";
                            btnTake.IsEnabled = false;
                        }
                        else
                        {
                            txtName.Text = arg.Info.Name;
                            txtInfo.Text = arg.Info.ToString();
                            if (CanTakeFood(arg.Info.Calories))
                            {
                                txtSuggest.Text = "您可以吃该食物";
                                txtSuggest.Foreground = new SolidColorBrush(Colors.Green);
                            }
                            else
                            {

                                txtSuggest.Text = "您最好不要吃该食物";
                                txtSuggest.Foreground = new SolidColorBrush(Colors.Red);
                            }

                            btnTake.IsEnabled = true;
                        }
                    }
                });
                lib.GetFoodByBarcodeAsync(e.Code);
            }
        }

        void barcodeSanner1_ScanBegin(object sender, Controls.BarcodeScanBeginArgs e)
        {
            if (!IsScanning)
            {
                barcodeSanner1.TestScan();
                txtName.Text = "扫描中...";
                txtSuggest.Text = "";
                txtInfo.Text = "";
            }
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
                if (MessageBox.Show(string.Format("开始 {0} ?", Item.Name), "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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

        private void btnTake_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFood != null)
            {
                ScheduleItem item = Schedule.AddItem(
                      new ScheduleItem()
                      {
                          Name = "加餐：" + CurrentFood.Name,
                          Calorie = CurrentFood.Calories,
                          OriginTime = DateTime.Now,
                          Type = ScheduleItemType.Diets
                      });
                if (CurrentFood.Calories > 0)
                    item.Details.Add("卡路里：" + CurrentFood.Calories.ToString("0.00") + "大卡");
                if (CurrentFood.Carbohydrate > 0)
                    item.Details.Add("碳水化合物：" + CurrentFood.Carbohydrate.ToString("0.00") + "g");
                if (CurrentFood.Fat > 0)
                    item.Details.Add("脂肪：" + CurrentFood.Fat.ToString("0.00") + "g");
                if (CurrentFood.Protein > 0)
                    item.Details.Add("蛋白质：" + CurrentFood.Protein.ToString("0.00") + "g");
            }
        }
    }
}