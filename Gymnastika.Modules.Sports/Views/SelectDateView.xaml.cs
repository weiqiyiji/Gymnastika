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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Sports.Events;


namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for SelectDateView.xaml
    /// </summary>
    public partial class SelectDateView : Window
    {
        //Just a funny name
        private bool _iAmReallyWantToClose = false;

        public SelectDateView()
        {
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_iAmReallyWantToClose && !e.Cancel)
            {
                e.Cancel = true;
                this.RaiseEvent(new RoutedEventArgs(RoutedClosingEvent));
            }
        }

        public static readonly RoutedEvent RoutedClosingEvent = EventManager.RegisterRoutedEvent(
          "RoutedClosing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SelectDateView));

        public event RoutedEventHandler RoutedClosing
        {
            add
            {
                base.AddHandler(SelectDateView.RoutedClosingEvent, value);
            }
            remove
            {
                base.RemoveHandler(SelectDateView.RoutedClosingEvent, value);
            }
        }

        private void OnWindowClosingStoryboard_Completed(object sender, EventArgs e)
        {
            _iAmReallyWantToClose = true;
            this.Close();
        }

        private void calendar1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime)e.AddedItems[0];
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<SportSelectDateEvent>().Publish(date);
            Date = date;
            this.Close();
        }

        public DateTime Date
        {
            get;
            set;
        }


    }
}
