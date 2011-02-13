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
    public static class Grade
    {
        public class GradeItem
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
            public string Remarks { get; set; }
        }
        public static GradeItem[] GradeSchedule(Schedule.ScheduleItem[] Items)
        {
            List<GradeItem> items = new List<GradeItem>();
            return items.ToArray();
        }
    }
}
