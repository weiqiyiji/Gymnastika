using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace Gymnastika.Modules.Sports.Validations
{
    public class TimeExpressionValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string exp = value.ToString();
            int hour,minute;
            
           return new ValidationResult(ValidateTime(exp, out hour, out minute),null);
        }

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
