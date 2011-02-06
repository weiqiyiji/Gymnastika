using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Gymnastika.Phone
{
    public partial class LoginProgressPage : PhoneApplicationPage
    {
        public LoginProgressPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            UserProfile.UserProfileManager.ActiveProfile.OnLoginCompeleted += new UserProfile.Profile.OnLoginCompeleteHandler(ActiveProfile_OnLoginCompeleted);
            UserProfile.UserProfileManager.ActiveProfile.OnLoginError +=new UserProfile.Profile.OnLoginErrorHandler(ActiveProfile_OnLoginError);
            UserProfile.UserProfileManager.ActiveProfile.OnLoginProgressChanged += new UserProfile.Profile.OnLoginProgressChangedHandler(ActiveProfile_OnLoginProgressChanged);
            UserProfile.UserProfileManager.ActiveProfile.BeginLogin();
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(LoginProgressPage_BackKeyPress);
        }

        void LoginProgressPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UserProfile.UserProfileManager.ActiveProfile.AbortLogin();
        }

        void ActiveProfile_OnLoginProgressChanged(object sender, string Progress)
        {
            this.Dispatcher.BeginInvoke(delegate
            {
                this.txtStatus.Text = Progress;
            }
            );
        }

        void ActiveProfile_OnLoginError(object sender,Exception ex,bool CanRetry,bool Aborted)
        {
            if (Aborted)
            {
                NavigationService.GoBack();
                return;
            }
            this.Dispatcher.BeginInvoke(delegate
            {
                txtStatus.Text = ex.Message;
            });
        }

        void ActiveProfile_OnLoginCompeleted(object sender, bool seccussful)
        {
            this.Dispatcher.BeginInvoke(delegate
            {
                NavigationService.GoBack();
            });
        }
    }
}