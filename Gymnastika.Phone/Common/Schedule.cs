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
namespace Gymnastika.Phone.Common
{
    #region sub class
    public enum ScheduleItemStatus
    {
        Done,
        Suspend,
        Pending,
        Active,
        Aborted
    }
    public class ScheduleItem:DependencyObject
    {
        //public FrameworkElement Content;
        public ScheduleItem()
        {
           // Content = new SchduleListItem(0, this);
            this.Duration = TimeSpan.FromSeconds(0);
        }
        private static string TranslateStatus(ScheduleItemStatus status)
        {
            
            switch(status)
            {
                case ScheduleItemStatus.Aborted:
                    return "被终止";
                case ScheduleItemStatus.Active:
                    return "进行中";
                case ScheduleItemStatus.Done:
                    return "已完成";
                case ScheduleItemStatus.Pending:
                    return "推迟中";
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
        public DateTime Time { get { return m_Time > OriginTime ? m_Time : OriginTime; } set { m_Time = value; this.SetValue(TimeTextProperty, Time.ToString("HH:mm:ss")); if (ScheduleContentChagned != null)ScheduleContentChagned(this, new EventArgs()); } }
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
    }
    #endregion
    public  class Schedule
    {

        public delegate void ItemStatusChangedHandler(object sender, ScheduleItemStatus OldStatus, ScheduleItemStatus NewStatus);
        public event ItemStatusChangedHandler ItemStatusChanged;
        private  List<ScheduleItem> m_Items = new List<ScheduleItem>();
        public  ScheduleItem[] GetAllSchedules()
        {
            List<ScheduleItem> items = new List<ScheduleItem>();
            return m_Items.ToArray();
        }
        public ScheduleItem AddItem(ScheduleItem Item)
        {
            Item.StatusChange += new ScheduleItem.StatusChangedHandler(Item_StatusChange);
            m_Items.Add(Item);
            return Item;
        }

        void Item_StatusChange(object sender, ScheduleItemStatus OldStatus, ScheduleItemStatus NewStatus)
        {
            if (ItemStatusChanged != null)
            {
                ItemStatusChanged.Invoke(sender, OldStatus, NewStatus);
            }
        }
        public  void UpdateStatus(ScheduleItem Item, ScheduleItemStatus Status)
        {
            Item.Status = Status;
        }
        public  void Delay(ScheduleItem Item, TimeSpan Span)
        {
            if (Item.Time < Item.OriginTime)
                Item.Time = Item.OriginTime;
            Item.Time += Span;
        }
        public void Suspend(ScheduleItem Item)
        {
            UpdateStatus(Item, ScheduleItemStatus.Suspend);
        }
        public  void GiveUp(ScheduleItem Item)
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
