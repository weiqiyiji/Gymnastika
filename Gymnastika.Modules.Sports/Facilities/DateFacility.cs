using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Facilities
{
    public static class DateFacility
    {
        public static string GetDayName(DayOfWeek day)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day);
        }

        public static string GetShortTime(DateTime time)
        {
            return time.ToString("t");
        }

        public static string GetShortTime(int Hour, int Minute)
        {
            return GetShortTime(new DateTime(1, 1, 1, Hour, Minute, 0));
        }

        public static string GetShortDate(DateTime time)
        {
            return time.ToString("MM月-dd日");
        }
    }
}
