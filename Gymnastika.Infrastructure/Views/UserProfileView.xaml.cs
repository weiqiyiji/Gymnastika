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
using System.Windows.Shapes;
using Gymnastika.ViewModels;
using Microsoft.Practices.Unity;

namespace Gymnastika.Views
{
    public partial class UserProfileView : Window, IUserProfileView
    {
        //Just a funny name
        private bool _iAmReallyWantToClose = false;

        public UserProfileView(UserProfileViewModel vm)
        {
            InitializeComponent();
            Model = vm;
            Owner = Application.Current.MainWindow;
        }

        public UserProfileViewModel Model
        {
            get
            {
                return DataContext as UserProfileViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        private void HeaderTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        	TextBlock block = (TextBlock)sender;
            block.Cursor = Cursors.Hand;
        }
		
		private void HeaderTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
		    TextBlock block = (TextBlock) sender;
		    block.Cursor = Cursors.Arrow;
		}

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if(!_iAmReallyWantToClose && !e.Cancel)
            {
                e.Cancel = true;
                this.RaiseEvent(new RoutedEventArgs(RoutedClosingEvent));
            }
        }
        
        public static readonly RoutedEvent RoutedClosingEvent = EventManager.RegisterRoutedEvent(
          "RoutedClosing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserProfileView));

        public event RoutedEventHandler RoutedClosing
        {
            add
            {
                base.AddHandler(UserProfileView.RoutedClosingEvent, value);
            }
            remove
            {
                base.RemoveHandler(UserProfileView.RoutedClosingEvent, value);
            }
        }

        private void OnWindowClosingStoryboard_Completed(object sender, EventArgs e)
        {
            _iAmReallyWantToClose = true;
            this.Close();
        }
					
    }
}
