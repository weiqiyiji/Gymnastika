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

namespace Gymnastika.Phone.UserProfile
{

    public class Profile
    {

        #region Events
        public delegate void OnLoginCompeleteHandler(object sender,bool seccussful);
        public delegate void OnLoginErrorHandler(object sender, Exception exception, bool CanRetry,bool Aborted);
        public delegate void OnLoginProgressChangedHandler(object sender, string Progress);
        public event OnLoginCompeleteHandler OnLoginCompeleted;
        public event OnLoginErrorHandler OnLoginError;
        public event OnLoginProgressChangedHandler OnLoginProgressChanged;
        #endregion
        public string Username { get; set; }
        public string Password { get; set; }
        public ImageSource Icon { get; set; }
        public bool AutoLogin { get; set; }
        public bool IsOnline { get; set; }
        public Gender Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Token { get; set; }
        public Profile(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        public Profile(string Username, string Password, ImageSource Icon)
            : this(Username, Password)
        {
            this.Icon = Icon;
        }
        public Profile(string Username,string Password,ImageSource Icon,
            Gender Gender,double Height,double Weight)
            :this(Username,Password,Icon)
        {
            this.Gender = Gender;
            this.Height = Height;
            this.Weight = Weight;
        }
        private void LoginCompeleted_()
        {
            Weight = 55;
            Height = 171;
            this.IsOnline = true;
            if (OnLoginCompeleted != null)
                OnLoginCompeleted.Invoke(this, true);
        }
        void doLogin()
        {
          //  Thread.Sleep(5000);
            LoginCompeleted_();
        }
        public void BeginLogin()
        {
            IsOnline = false;
            Thread th = new Thread(doLogin);
            th.Start();
        }
        public void AbortLogin()
        {
            if (OnLoginError != null)
                OnLoginError.Invoke(this, new Exception("User aborted."), false, true);
        }
    }
}
