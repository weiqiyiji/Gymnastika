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
using Gymnastika.Phone.UserProfile;
namespace Gymnastika.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);       
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(MainPage_BackKeyPress);
            
    
        }
        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
        
            base.OnOrientationChanged(e);
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
          }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = MessageBox.Show("退出？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (UserProfileManager.ActiveProfile == null)
            {
                Profile[] profiles = UserProfileManager.GetAllStoredProfiles();
                if (profiles.Length > 0)
                {
                    foreach (Profile profile in profiles)
                    {
                        if (profile.AutoLogin)
                        {
                            UserProfileManager.ActiveProfile = profile;
                            break;
                        }
                    }
                }
                if (UserProfileManager.ActiveProfile != null)
                {
                    NavigationService.Navigate(Pages.GetPageUri<LoginProgressPage>());
                }
                NavigationService.Navigate(Pages.GetPageUri<LoginPage>());
                return;
            }
            else if (!UserProfileManager.ActiveProfile.IsOnline)
            {
                NavigationService.Navigate(Pages.GetPageUri<LoginProgressPage>());
            }
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
           
            base.OnNavigatedFrom(e);
        }
    }
}