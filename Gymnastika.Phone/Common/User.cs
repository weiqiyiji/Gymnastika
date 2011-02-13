using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace Gymnastika.Phone.Common
{
    public class User
    {
        private class LoginCallbackOfProfile
        {
            public UserProfile.Profile Profile{get;set;}
            public LoginCallback Callback;
            public LoginCallbackOfProfile(UserProfile.Profile Profile, LoginCallback Callback)
            {
                this.Profile = Profile;
                this.Callback = Callback;
            }
        }
        public delegate void LoginCallback(UserProfile.Profile Profile,bool Success,string uss);
        public static void EndLogin()
        {
        }
        private static void OnLoginCompeleted(LoginCallbackOfProfile Callback,bool Success,string uss)
        {
            Callback.Callback.Invoke(Callback.Profile, Success, uss);
        }
        private static void LoginEntry(Object oLoginCallbackOfProfile)
        {
            Thread.Sleep(1000);
            LoginCallbackOfProfile callback = oLoginCallbackOfProfile as LoginCallbackOfProfile;
            OnLoginCompeleted(callback, true, "JustATest");
        }
        public static void BeginLogin(UserProfile.Profile Profile, LoginCallback Callback)
        {
            Thread LoginThread = new Thread(LoginEntry);
            LoginThread.Start(new LoginCallbackOfProfile(Profile, Callback));
        }
    }
}
