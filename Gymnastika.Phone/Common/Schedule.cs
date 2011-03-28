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
using System.Collections.Generic;
using Gymnastika.Phone.Controls;
using Gymnastika.Phone.Sync;
namespace Gymnastika.Phone.Common
{
    #region sub class
    public enum ScheduleItemStatus
    {
        Done,
        Suspend,
        Pending,
        Active,
        Aborted,
        Normal,
        Forward
    }
    public class ScheduleItem : DependencyObject
    {
        //public FrameworkElement Content;
        public ScheduleItem()
        {
            // Content = new SchduleListItem(0, this);
            this.Duration = TimeSpan.FromSeconds(0);
            OriginTime = DateTime.MinValue;
            m_Status = ScheduleItemStatus.Normal;

        }
        private static string TranslateStatus(ScheduleItemStatus status)
        {

            switch (status)
            {
                case ScheduleItemStatus.Aborted:
                    return "被终止";
                case ScheduleItemStatus.Active:
                    return "进行中";
                case ScheduleItemStatus.Done:
                    return "已完成";
                case ScheduleItemStatus.Pending:
                    return "推迟中";
                case ScheduleItemStatus.Suspend:
                    return "暂停中";
                case ScheduleItemStatus.Normal:
                    return "未开始";
                case ScheduleItemStatus.Forward:
                    return "提前";
                default:
                    return "未知状态";

            }
        }
        public static DependencyProperty NameProperty = DependencyProperty.Register("NameProperty", typeof(string), typeof(ScheduleItem), null);
        public static DependencyProperty TimeProperty = DependencyProperty.Register("TimeProperty", typeof(DateTime), typeof(ScheduleItem), null);
        public event EventHandler ScheduleContentChagned;
        public delegate void StatusChangedHandler(object sender, ScheduleItemStatus OldStatus, ScheduleItemStatus NewStatus);
        public event StatusChangedHandler StatusChange;
        public int ID { get; set; }
        public string Name { get { return (string)this.GetValue(NameProperty); } set { this.SetValue(NameProperty, value); if (ScheduleContentChagned != null) ScheduleContentChagned(this, new EventArgs()); } }
        public DateTime OriginTime { get; set; }
        public Duration Duration { get; set; }
        private DateTime m_Time;
        public DateTime Time { get { return m_Time; } set { m_Time = value; if (OriginTime == DateTime.MinValue) OriginTime = value; ;this.SetValue(TimeTextProperty, Time.ToString("HH:mm:ss")); if (ScheduleContentChagned != null)ScheduleContentChagned(this, new EventArgs()); } }
        private ScheduleItemStatus m_Status;
        public ScheduleItemStatus Status
        {
            get { return m_Status; }
            set
            {
                ScheduleItemStatus old = m_Status;

                m_Status = value;
                this.SetValue(StatusTextProperty, TranslateStatus(m_Status));
                if (StatusChange != null)
                {
                    StatusChange.Invoke(this, old, value);
                }
                if (ScheduleContentChagned != null)
                    ScheduleContentChagned(this, new EventArgs());
            }
        }
        public ImageSource Icon { get; set; }
        public string StatusText { get { return this.GetValue(StatusTextProperty) as string; } }
        public string TimeText { get { return this.GetValue(TimeTextProperty) as string; } }
        public double Calorie { get; set; }
        public double Point { get; set; }
        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register("StatusText", typeof(string), typeof(ScheduleItem), new PropertyMetadata(""));
        public static readonly DependencyProperty TimeTextProperty = DependencyProperty.Register("TimeText", typeof(string), typeof(ScheduleItem), new PropertyMetadata(""));
        public void Delay(TimeSpan time)
        {

            this.Time += time;
            if (this.Time > this.OriginTime)
                this.Status = ScheduleItemStatus.Pending;
            else if (this.Time < this.OriginTime)
                this.Status = ScheduleItemStatus.Forward;
            else
                this.Status = ScheduleItemStatus.Normal;
        }
    }
    #endregion
    public class Schedule
    {

        public delegate void ItemStatusChangedHandler(object sender, ScheduleItemStatus OldStatus, ScheduleItemStatus NewStatus);
        public static event ItemStatusChangedHandler ItemStatusChanged;
        private static List<ScheduleItem> m_Items = new List<ScheduleItem>();
        public static void SyncPlan(string userID)
        {
            HttpClient client = new HttpClient();
            client.GetCompeleted += new EventHandler<HttpClient.GetCompeletedArgs>(client_GetCompeleted);
            client.Get(Config.GetServerPathUri(
                string.Format(Config.GetPlanOfTodayServiceUri, userID)),null);
        }
        static void ParseSchdule(string xml)
        {

        }
        static void client_GetCompeleted(object sender, HttpClient.GetCompeletedArgs e)
        {
            if (e.Error != null)
            {
                ParseSchdule(e.Result);
            }
            else
            {
                App.Current.RootVisual.Dispatcher.BeginInvoke(
                    delegate { MessageBox.Show("同步计划列表失败。"); });
            }
        }
        public static ScheduleItem[] GetAllSchedules()
        {
            List<ScheduleItem> items = new List<ScheduleItem>();
            return m_Items.ToArray();
        }
        private class SchduleComparision : IComparer<ScheduleItem>
        {
            public int Compare(ScheduleItem x, ScheduleItem y)
            {
                if (x.Time > y.Time)
                    return 1;
                else if (x.Time < y.Time)
                    return -1;
                else
                    return 0;
            }
        }
        public static void Sort()
        {
            m_Items.Sort(new SchduleComparision());
        }
        public static ScheduleItem GetNextSchduleItem()
        {
            Sort();
            for (int i = 0; i < m_Items.Count - 1; i++)
            {
                if (m_Items[i].Time <= DateTime.Now && m_Items[i + 1].Time >= DateTime.Now)
                    return m_Items[i];
            }
            if (m_Items[m_Items.Count - 1].Time >= DateTime.Now)
                return m_Items[m_Items.Count - 1];
            return null;
        }
        public static ScheduleItem AddItem(ScheduleItem Item)
        {
            Item.StatusChange += new ScheduleItem.StatusChangedHandler(Item_StatusChange);
            m_Items.Add(Item);
            return Item;
        }

        static void Item_StatusChange(object sender, ScheduleItemStatus OldStatus, ScheduleItemStatus NewStatus)
        {
            if (ItemStatusChanged != null)
            {
                ItemStatusChanged.Invoke(sender, OldStatus, NewStatus);
            }
        }
        public static void UpdateStatus(ScheduleItem Item, ScheduleItemStatus Status)
        {
            Item.Status = Status;
        }
        public static void Delay(ScheduleItem Item, TimeSpan Span)
        {
            Item.Delay(Span);
        }

        public static void Suspend(ScheduleItem Item)
        {
            UpdateStatus(Item, ScheduleItemStatus.Suspend);
        }
        public static void GiveUp(ScheduleItem Item)
        {
            UpdateStatus(Item, ScheduleItemStatus.Aborted);
        }
        public static Schedule ActiveSchedule
        {
            get { return GetSchedule(DateTime.Today); }
        }
        public static Schedule GetSchedule(DateTime Time)
        {
            return new Schedule();
        }
    }
}
