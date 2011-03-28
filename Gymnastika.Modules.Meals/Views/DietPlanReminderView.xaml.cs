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
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for DietPlanReminderView.xaml
    /// </summary>
    public partial class DietPlanReminderView
    {
        private bool _iAmReallyWantToClose = false;

        public DietPlanReminderView()
        {
            InitializeComponent();
        }

        #region IDietPlanReminderView Members

        public DietPlanReminderViewModel Context
        {
            get
            {
                return this.DataContext as DietPlanReminderViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion

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
          "RoutedClosing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DietPlanReminderView));

        public event RoutedEventHandler RoutedClosing
        {
            add
            {
                base.AddHandler(DietPlanReminderView.RoutedClosingEvent, value);
            }
            remove
            {
                base.RemoveHandler(DietPlanReminderView.RoutedClosingEvent, value);
            }
        }

        private void OnWindowClosingStoryboard_Completed(object sender, EventArgs e)
        {
            _iAmReallyWantToClose = true;
            this.Close();
        }
    }
}
