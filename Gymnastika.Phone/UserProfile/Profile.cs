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
using System.IO;
using System.Text.RegularExpressions;

namespace Gymnastika.Phone.UserProfile
{

    public class Profile
    {
        #region Static Members
        public delegate void RegisterCompeleted(string Username, Exception error);
        private static void OnRegisterCompeleted(string Username, Exception ex, RegisterCompeleted callback)
        {
            callback(Username, ex);
        }
        public static void Register(string Username, string Password, RegisterCompeleted callback)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(
                new Uri(Config.ServerUri,
                    string.Format("/regsiter.aspx?u=wp7&username={0}&pwd={1}",
                   HttpUtility.UrlEncode(Username), HttpUtility.UrlEncode(Password))));
            request.BeginGetResponse(
                new AsyncCallback(
                    (r) =>
                    {
                        if (request.HaveResponse)
                        {
                            using (StreamReader reader = new StreamReader(request.EndGetResponse(r).GetResponseStream()))
                            {
                                string response = reader.ReadToEnd();
                                if (response.Contains(","))
                                {
                                    string[] strs = new string[2];
                                    strs[0] = response.Substring(0, response.IndexOf(",")).ToLower();
                                    strs[1] = response.Substring(response.IndexOf(",") + 1);
                                    if (strs[0] == "failed")
                                    {
                                        OnRegisterCompeleted(Username, new Exception(strs[1]), callback);
                                    }
                                    else if (strs[0] == "ok")
                                    {
                                        OnRegisterCompeleted(Username, null, callback);
                                    }
                                    else
                                        OnRegisterCompeleted(Username, new Exception("未知回复。"), callback);
                                }
                                else
                                    OnRegisterCompeleted(Username, new Exception("未知回复。"), callback);
                            }
                        }
                        else
                            OnRegisterCompeleted(Username, new Exception("服务器无回复。"), callback);
                    }
                )
                , request);
        }


        public delegate void SignInCompeleted(string Username, Exception error);

        public static void SignIn(string Username, string Password, SignInCompeleted Callback)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(
                new Uri(Config.ServerUri,
                    string.Format("/LoginIn.aspx?u=wp7&username={0}&pwd={1}", HttpUtility.UrlEncode(Username),
                    HttpUtility.UrlEncode(Password))));
            request.BeginGetResponse(
                new AsyncCallback(
                    (r) =>
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)request.EndGetResponse(r);
                        using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            string response = reader.ReadToEnd();
                            if (response.Contains(","))
                            {
                                string[] strs = new string[2];
                                strs[0] = response.Substring(0, response.IndexOf(",")).ToLower();
                                strs[1] = response.Substring(response.IndexOf(",") + 1);
                                if (strs[0] == "ok")
                                    Callback(Username, null);
                                else if (strs[0] == "failed")
                                    Callback(Username, new Exception(strs[1]));
                                else
                                    Callback(Username, new Exception("未知回复。"));
                            }
                            else
                                Callback(Username, new Exception("未知回复。"));
                        }
                    }
            ), request);
        }
        #endregion
        #region Events
        public delegate void OnLoginCompeleteHandler(object sender, bool successful);
        public delegate void OnLoginErrorHandler(object sender, Exception exception, bool CanRetry, bool Aborted);
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
        public Profile(string Username, string Password, ImageSource Icon,
            Gender Gender, double Height, double Weight)
            : this(Username, Password, Icon)
        {
            this.Gender = Gender;
            this.Height = Height;
            this.Weight = Weight;
        }
        private void LoginCompeleted_()
        {
            Weight = 55;
            Height = 171;
            UpdateLoginPorgress("登录成功。");
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
            SignIn(Username, Password, new SignInCompeleted(
                (str, ex) =>
                { OnLoginCompeleted(this, ex != null); }
                ));

            //Thread th = new Thread(doLogin);
            //th.Start();
            //UpdateLoginPorgress("正在登录...");

        }
        private void UpdateLoginPorgress(string msg)
        {
            if (OnLoginProgressChanged != null)
                OnLoginProgressChanged.Invoke(this, msg);
        }
        public void AbortLogin()
        {
            UpdateLoginPorgress("正在终止...");
            if (OnLoginError != null)
                OnLoginError.Invoke(this, new Exception("User aborted."), false, true);
        }
    }
}
