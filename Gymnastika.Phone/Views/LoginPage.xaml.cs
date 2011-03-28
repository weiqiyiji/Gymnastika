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
using Gymnastika.Phone.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls.Primitives;
using Gymnastika.Phone.UserProfile;
using System.IO;
using System.Text;

namespace Gymnastika.Phone.Views
{
    public partial class LoginPage : PhoneApplicationPage
    {

        private List<UserProfile.Profile> Users = new List<UserProfile.Profile>();
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ClearUserList();
            if (State.ContainsKey("ActiveProfile"))
                UserProfileManager.ActiveProfile = State["ActiveProfile"] as Profile;
            
            foreach (UserProfile.Profile p in UserProfileManager.GetAllStoredProfiles())
            {
                AddUser(p);
            }
            base.OnNavigatedTo(e);
        }
        void ClearUserList()
        {
            spUsers.Children.Clear();
        }
        public LoginPage()
        {
            InitializeComponent();
            Common.DefualtTransition.SetNavigationTransition(this);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(LoginPage_ManipulationCompleted);
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(LoginPage_ManipulationStarted);
            CurrentOffsetY = 0;
            Arrange(CurrentOffsetY);
         
        }
        void AddUser(UserProfile.Profile profile)
        {
            UserSelectorItem item = new UserSelectorItem()
            {
                Username = profile.Username,
                UserIcon=profile.Icon,
                Profile=profile
            };
           
            item.DeleteClick += new EventHandler<EventArgs>(item_DeleteClick);
            item.SiginClick += new EventHandler<EventArgs>(item_SiginClick);
            spUsers.Children.Add(item);
        }
        void RemoveUser(UserSelectorItem item)
        {
            spUsers.Children.Remove(item);
        }
        void item_SiginClick(object sender, EventArgs e)
        {
            UserSelectorItem item = sender as UserSelectorItem;
            item.Profile.OnLoginCompeleted += new Profile.OnLoginCompeleteHandler(Profile_OnLoginCompeleted);
            item.Profile.BeginLogin();
        }

        void Profile_OnLoginCompeleted(object sender, bool successful)
        {
            this.Dispatcher.BeginInvoke(delegate
            {
                UserProfileManager.ActiveProfile = sender as Profile;
                NavigationService.Navigate(Pages.GetPageUri<MainPage>());
            });
        }

        void item_DeleteClick(object sender, EventArgs e)
        {
            UserSelectorItem item = sender as UserSelectorItem;
            RemoveUser(item);
            UserProfileManager.DeleteStoredPofile(item.Profile);
        }

        void LoginPage_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {

        }

        void LoginPage_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }
        List<Image> imgs = new List<Image>();

        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.GetPageUri<AddAccountPage>());
        }
        private double CurrentOffsetY;
        private void Arrange(double OffsetY)
        {

        }
        private void ScrollToNearest()
        {
        }

        void cw_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserProfileManager.ActiveProfile != null)
                NavigationService.Navigate(Pages.GetPageUri<MainPage>());
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.GetPageUri<MainPage>());
        }
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (State.ContainsKey("ActiveProfile"))
            {
                State["ActiveProfile"] = UserProfileManager.ActiveProfile;
            }else{
                State.Add("ActiveProfile", UserProfileManager.ActiveProfile);
            }
            base.OnNavigatingFrom(e);
        }
    }
}