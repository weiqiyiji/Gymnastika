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
        private static string GetRemarks(ScheduleItem Item)
        {
            switch (Item.Status)
            {
                case ScheduleItemStatus.Aborted:
                    return "被终止";
                case ScheduleItemStatus.Done:
                    return "顺利完成";
                case ScheduleItemStatus.Suspend:
                    return "推迟中";
                case ScheduleItemStatus.Pending:
                    return "等待中";
                case ScheduleItemStatus.Active:
                    return "进行中";
                default:
                    return string.Empty;
            }
        }
        public static GradeItem[] GradeSchedule(ScheduleItem[] Items)
        {
            List<GradeItem> gradeItems = new List<GradeItem>();
            for (int i = 0; i <Items.Length; i++)
            {
                gradeItems.Add(new GradeItem()
                {
                    Index=i+1,
                    Name=Items[i].Name,
                    Value=(Items[i].Status==ScheduleItemStatus.Done?Items[i].Point:0),
                    Remarks=GetRemarks(Items[i])
                });
            }
            return gradeItems.ToArray();
        }
    }
}
