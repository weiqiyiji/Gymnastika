using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace Gymnastika.Modules.Sports.Converters
{
    public class HourMinuteToStringConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int hour = (int)values[0];
            int minute = (int)values[1];
            if (IsTimeValid(hour, minute))
            {
                return string.Format("{0}:{1}", hour.ToString(), minute.ToString("00"));
            }
            else
            {
                return "";
                throw new Exception("Invalid Time Range");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            string exp = value.ToString();
            int hour,minute;
            if (ValidateTime(exp, out hour, out minute))
            {
                return new object[2] { hour, minute };
            }
            else
            {
                return new object[2] { 25, 25 };
                throw new Exception("Invalid Time Formate");
            }
        }

        #endregion

        bool ValidateTime(string expression, out int hour, out int minute)
        {
            hour = 0;
            minute = 0;
            try
            {
                const string patten = "(\\d{0,2}):(\\d{0,2})";
                MatchCollection collection = Regex.Matches(expression, patten);
                if (collection.Count != 1)
                    return false;
                else
                {
                    Match match = collection[0];
                    hour = Int32.Parse(match.Groups[1].Value);
                    minute = Int32.Parse(match.Groups[2].Value);
                    return IsTimeValid(hour, minute);
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        bool IsTimeValid(int hour, int minute)
        {
            return (hour >= 0 && minute >= 0 && minute < 60 && hour * 60 + minute <= 24 * 60);
        }
    }
}
