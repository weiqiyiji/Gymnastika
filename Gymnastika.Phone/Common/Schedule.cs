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
namespace Gymnastika.Phone.Common
{
    public static class Schedule
    {
        public enum ScheduleItemStatus
        {
            Done,
            Suspend,
            Inactive,
            Active
        }
        public class ScheduleItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime Time { get; set; }
            public ScheduleItemStatus Status { get; set; }
            public ImageSource Icon { get; set; }
            public double Calorie { get; set; }
        }
        public static ScheduleItem[] GetAllSchedules()
        {
            List<ScheduleItem> items = new List<ScheduleItem>();
            return items.ToArray();
        }
        public static void UpdateStatus(ScheduleItem Item, ScheduleItemStatus Status)
        {

        }
        public static void Delay(ScheduleItem Item, TimeSpan Span)
        {

        }
    }
}
