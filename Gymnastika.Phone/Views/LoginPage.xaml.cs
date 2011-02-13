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
using Microsoft.Phone.Shell;
using Gymnastika.Phone.UserProfile;
namespace Gymnastika.Phone
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public enum LoginReasons
        {
            NoUserProfile,
            SwitchUser,
            NeedPassword
        }
        public enum CancelActions
        {
            GoBack,
            ExitApplication
        }
        public enum ArgsType
        {
            CancelAction,
            LoginReason,
            None,
            Both
        }
        public CancelActions CancelAction { get; set; }
        public LoginReasons LoginReason { get; set; }
        public bool LoginOK { get; protected set; }
        public LoginPage()
        {
            InitializeComponent();
            txtReg.Text = "还没有帐号？\r\n\t请到桌面端注册。";
            LoginOK = false;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (UserProfileManager.ActiveProfile != null &&
                UserProfileManager.ActiveProfile.IsOnline &&
                NavigationService.CanGoBack)
                NavigationService.GoBack();
            if (State.ContainsKey("Username"))
                txtUsername.Text = (string)State["Username"];
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (UserProfileManager.ActiveProfile == null)
                UserProfileManager.ActiveProfile = new Profile(txtUsername.Text, txtPassword.Password);
            base.OnNavigatedFrom(e);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserProfileManager.ActiveProfile = new Profile(txtUsername.Text.Trim(), txtPassword.Password) { AutoLogin = cbAutoLogin.IsChecked==true };
            State["Username"] = txtUsername.Text;
            NavigationService.Navigate(Pages.GetPageUri<LoginProgressPage>());

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}