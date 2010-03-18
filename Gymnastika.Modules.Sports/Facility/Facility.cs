using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Facility
{
    public class Facility
    {
        static public bool TheSameDay(DateTime time, int year, int month, int day)
        {
            return time.Year == year && time.Month == month && time.Day == day;
        }

        static public bool TheSameDay(DateTime time,SportsPlan plan)
        {
            return TheSameDay(time, plan.Year, plan.Month, plan.Day);
        }

        static public bool TheSameDay(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }

        static public bool TheSameWeek(DateTime time, int year, int month, int day)
        {
            DateTime date = new DateTime(year, month, day);
            DateTime Sunday1 = date.AddDays(-(int)date.DayOfWeek);
            DateTime Sunday2 = time.AddDays(-(int)time.DayOfWeek);
            return TheSameDay(Sunday1, Sunday2);
        
        }

        static public bool TheSameWeek(DateTime date, SportsPlan plan)
        {
            return TheSameWeek(date, plan.Year, plan.Month, plan.Day);
        }

        static public DateTime Sunday(DateTime time)
        {
            return time.AddDays(-(int)time.DayOfWeek);
        }

        static public DateTime Sunday(int year, int month, int day)
        {
            return Sunday(new DateTime(year, month, day));
        }
    }
}
