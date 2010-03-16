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

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for CalendarButtonView.xaml
    /// </summary>
    public partial class CalendarButtonView : UserControl
    {
        public CalendarButtonView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(DataContext.ToString());
        }
    }

    public class PlanTemplateSelecter : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
            FrameworkElement element = container as FrameworkElement;
            ICalendarButtonViewModel viewmodel = item as ICalendarButtonViewModel;
            if (viewmodel.Plan == null)
                return element.FindResource("NoPlanTemplate") as DataTemplate;
            else
                return element.FindResource("WithPlanTemplate") as DataTemplate;
        }
    }
}
