using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Services.Providers;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Sports.Models;
using System.Windows;

namespace Gymnastika.Modules.Sports.Converters
{
    public class TimeToPlanConverter : DependencyObject, IValueConverter
    {


        public IList<SportsPlan> Plans
        {
            get 
            {
                var val = GetValue(PlansProperty);
                return val as IList<SportsPlan>;
            }
            set { SetValue(PlansProperty, value); }
        }

        public static readonly DependencyProperty PlansProperty =
            DependencyProperty.Register("Plans", typeof(IList<SportsPlan>), typeof(TimeToPlanConverter));

        public TimeToPlanConverter()
        {
            if (Plans == null)
                Plans = new List<SportsPlan>();
        }

        bool TheSameDay(DateTime date,SportsPlan plan)
        {
            return date.Year == plan.Year && date.Month == plan.Month && date.Day == plan.Day;
        }

        #region IValueConverter Members


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return null;
            return new CalendarButtonViewModel(Plans.Where(t => TheSameDay((DateTime)value, t)).FirstOrDefault());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
