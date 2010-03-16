using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gymnastika.Modules.Sports.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Sports.Converters;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public CalendarView()
        {
            ViewModel = ServiceLocator.Current.GetInstance<ICalendarViewModel>();
            var converter = new TimeToPlanConverter();
            converter.Plans = ViewModel.Plans;
            this.Resources.Add("TimeToPlanConverter", converter); 
            InitializeComponent();
        }

        public ICalendarViewModel ViewModel
        {
            set { DataContext = value; }
            get { return DataContext as ICalendarViewModel; }
        }

    }
}
