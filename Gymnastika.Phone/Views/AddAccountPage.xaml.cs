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
using System.Text.RegularExpressions;
using Gymnastika.Phone.UserProfile;
using Gymnastika.Phone.Transitions;

namespace Gymnastika.Phone.Views
{
    public partial class AddAccountPage : PhoneApplicationPage
    {

        private List<FrameworkElement> AddAccountUIElements = new List<FrameworkElement>();
        private List<FrameworkElement> LoginProgressUIElements = new List<FrameworkElement>();
        private bool m_IsUsernameOK = false;
        private bool m_IsToLoginProgress = false;
        public AddAccountPage()
        {
            InitializeComponent();
            Common.DefualtTransition.SetNavigationTransition(this);
            IsSignInMode = true;
            SignInPanel.Visibility = Visibility.Visible;
            SignInProgressPanel.Visibility = Visibility.Collapsed;
        }
        private PlaneProjection GetPlaneProjection(FrameworkElement fe)
        {
            if (fe.Projection is PlaneProjection)
                return fe.Projection as PlaneProjection;
            return null;
        }

        #region Mode Control
        private bool IsSignInMode
        {
            get { return rbSignIn.IsChecked == true; }
            set
            {
                txtPasswordRepeat.Visibility = lblPasswordRepeat.Visibility = txtErrPasswordRepeat.Visibility
                    = value ? Visibility.Collapsed : Visibility.Visible;
                txtPasswordRepeat.Password = txtPassword.Password = "";
                rbSignIn.IsChecked = value;
                rbRegister.IsChecked = !value;
                CheckForm();
            }
        }
        private void rbSignIn_Checked(object sender, RoutedEventArgs e)
        {
            IsSignInMode = true;
            HideAllError();
        }

        private void rbRegister_Checked(object sender, RoutedEventArgs e)
        {
            IsSignInMode = false;
            HideAllError();
        }

        #endregion

        #region Error Display
        private void HideAllError()
        {
            txtErrPassword.Visibility = txtErrPasswordRepeat.Visibility = txtErrUsername.Visibility = Visibility.Collapsed;
        }
        private void ShowUsernameError(string err, bool passed)
        {
            txtErrUsername.Foreground = passed ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Red);
            txtErrUsername.Text = err;
            txtErrUsername.Visibility = Visibility.Visible;
        }
        private void ShowPasswordError(string err)
        {
            txtErrPassword.Text = err;
            txtErrPassword.Visibility = Visibility.Visible;
        }
        private void ShowPasswordRepeatError(string err)
        {
            txtErrPasswordRepeat.Text = err;
            txtErrPasswordRepeat.Visibility = Visibility.Visible;
        }

        #endregion

        #region Check Username

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                ShowUsernameError("必须输入用户名", false);
            }
            if (IsSignInMode)
            {
                m_IsUsernameOK = false;
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
                wc.DownloadStringAsync(new Uri(Config.ServerUri, "/checkform.aspx?c=usernmae&value=" +
                    HttpUtility.UrlEncode(txtUsername.Text)));
            }
            else
            {
                m_IsUsernameOK = txtUsername.Text.Length > 0;
            }
            CheckForm();
        }
        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtErrUsername.Visibility = Visibility.Collapsed;
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                delegate
                {
                    if (e.Error == null && !string.IsNullOrEmpty(e.Result))
                    {
                        string[] ret = Regex.Split(e.Result, @"\n");

                        if (ret.Length > 1)
                        {
                            if (ret[0] == txtUsername.Text)
                            {
                                ShowUsernameError(ret[1], false);
                            }
                            else
                            {
                                m_IsUsernameOK = true;
                                ShowUsernameError("该用户名可以使用。", true);
                            }
                            CheckForm();
                        }
                    }
                }
                );
        }

        #endregion

        #region Check Password

        private void txtPasswordRepeat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordRepeat.Password.Length == 0)
            {
                ShowPasswordRepeatError("请再次输入密码以确认。");
            }
            else if (txtPasswordRepeat.Password != txtPassword.Password)
            {
                ShowPasswordError("两次输入的密码不一致。");
                ShowPasswordRepeatError("两次输入的密码不一致。");
            }
            CheckForm();
        }

        private void txtPasswordRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtErrPassword.Visibility = Visibility.Collapsed;
            txtErrPasswordRepeat.Visibility = Visibility.Collapsed;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length == 0)
            {
                ShowPasswordError("必须输入密码。");
            }
            else if (!IsSignInMode && txtPasswordRepeat.Password.Length > 0 && txtPasswordRepeat.Password != txtPassword.Password)
            {
                ShowPasswordError("两次输入的密码不一致。");
                ShowPasswordRepeatError("两次输入的密码不一致。");
            }
            CheckForm();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtErrPassword.Visibility = Visibility.Collapsed;
            txtErrPasswordRepeat.Visibility = Visibility.Collapsed;
        }

        #endregion


        #region Check Form

        private bool CheckForm()
        {
            bool flag = false;
            if (IsSignInMode)
            {
                flag = txtUsername.Text.Length > 0 && txtPassword.Password.Length > 0;
            }
            else
            {
                flag = m_IsUsernameOK && txtPassword.Password.Length > 0 && txtPassword.Password == txtPasswordRepeat.Password;
            }
            btnNext.IsEnabled = flag;
            return flag;
        }

        #endregion

        #region Animaions
        private void AnimateShow(FrameworkElement fe)
        {
            LinearGradientBrush op = new LinearGradientBrush();
            GradientStop start = new GradientStop()
            {
                Offset = 0,
                Color = Color.FromArgb(255, 255, 255, 255)
            };
            GradientStop center = new GradientStop()
            {
                Offset = 0.01,
                Color = Color.FromArgb(0, 255, 255, 255)
            };
            op.GradientStops.Add(start);
            op.GradientStops.Add(center);
            op.GradientStops.Add(new GradientStop()
            {
                Offset = 1,
                Color = Color.FromArgb(0, 255, 255, 255)
            });
            fe.OpacityMask = op;
            Storyboard sb = new Storyboard();
            DoubleAnimation ani = new DoubleAnimation();
            DoubleAnimation ani2 = new DoubleAnimation();
            Storyboard.SetTarget(ani, center);
            Storyboard.SetTargetProperty(ani, new PropertyPath(GradientStop.OffsetProperty));
            Storyboard.SetTarget(ani2, start);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(GradientStop.OffsetProperty));

            ani2.From = 0;
            ani2.To = 1;

            ani.From = 0.001;
            ani.To = 1;

            ani.Duration = TimeSpan.FromSeconds(.3);
            ani2.Duration = TimeSpan.FromSeconds(.3);
            ani2.BeginTime = TimeSpan.FromSeconds(.2);
            sb.Children.Add(ani);
            sb.Children.Add(ani2);
            sb.Begin();
            sb.Completed += new EventHandler(sb_Completed);
        }

        private void AnimateHide(FrameworkElement fe)
        {
            Storyboard sb = new Storyboard();
            LinearGradientBrush op = new LinearGradientBrush();
            GradientStop start = new GradientStop()
            {
                Offset = 0,
                Color = Color.FromArgb(0, 255, 255, 255)
            };
            GradientStop center = new GradientStop()
            {
                Offset = 0.01,
                Color = Color.FromArgb(255, 255, 255, 255)
            };
            op.GradientStops.Add(start);
            op.GradientStops.Add(center);
            op.GradientStops.Add(new GradientStop()
            {
                Offset = 1,
                Color = Colors.White
            });
            fe.OpacityMask = op;

            DoubleAnimation ani = new DoubleAnimation();
            DoubleAnimation ani2 = new DoubleAnimation();
            Storyboard.SetTarget(ani, center);
            Storyboard.SetTargetProperty(ani, new PropertyPath(GradientStop.OffsetProperty));
            Storyboard.SetTarget(ani2, start);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(GradientStop.OffsetProperty));
            ani2.From = 0;
            ani2.To = 1;

            ani.From = 0.001;
            ani.To = 1;

            ani.Duration = TimeSpan.FromSeconds(.3);
            ani2.Duration = TimeSpan.FromSeconds(.3);
            ani2.BeginTime = TimeSpan.FromSeconds(.2);
            sb.Children.Add(ani);
            sb.Children.Add(ani2);
            sb.Begin();
            sb.Completed += new EventHandler(sb_Completed);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            if (m_IsToLoginProgress)
            {
                if (SignInPanel.Visibility != Visibility.Collapsed)
                {
                    SignInPanel.Visibility = Visibility.Collapsed;
                    AnimateShow(SignInProgressPanel);
                    SignInProgressPanel.Visibility = Visibility.Visible;

                }
                else
                {
                    SignInProgressPanel.OpacityMask = null;
                    if (IsSignInMode)
                        SignIn(txtUsername.Text, txtPassword.Password);
                    else
                        Register(txtUsername.Text, txtPassword.Password);
                }
            }
            else
            {
                if (SignInProgressPanel.Visibility != Visibility.Collapsed)
                {
                    SignInProgressPanel.Visibility = Visibility.Collapsed;
                    AnimateShow(SignInPanel);
                    SignInPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    SignInPanel.OpacityMask = null;
                }
            }
        }
        #endregion

        #region Register and sign in
        bool isRegisterSucceed = false;
        private void Register(string Username, string Password)
        {
#if DEBUG
             UserProfileManager.StoreProfice(new Profile(Username, Password));
            return;
#endif
            Profile.Register(Username, Password,
                new Profile.RegisterCompeleted((user, error) =>
                    {
                        pbProcessing.Visibility = Visibility.Collapsed;
                        if (error == null)
                        {
                            txtProcessingText.Text = string.Format("您已成功注册 {0}！\r\n登录中...", user);
                            UserProfileManager.StoreProfice(new Profile(Username, Password));
                            SignIn(Username, Password);
                            isRegisterSucceed = true;

                        }
                        else
                        {
                            isRegisterSucceed = false;
                            txtProcessingText.Text = string.Format("{0} 注册失败：{1}", user, error.Message);
                        }
                    }
            ));
        }
        private void SignIn(string Username, string Password)
        {
            pbProcessing.Visibility = Visibility.Visible;
            Profile.SignIn(Username, Password,
                new Profile.SignInCompeleted((user, error) =>
                    {
                        pbProcessing.Visibility = Visibility.Collapsed;
                        if (error == null)
                        {
                            UserProfileManager.ActiveProfile = UserProfileManager.GetStoredProfile(Username);
                            NavigationService.GoBack();
                        }
                        else
                        {
                            txtProcessingText.Text = string.Format("{0} 登录失败：{1}", user, error.Message);
                        }
                    }
            ));
        }
        #endregion
        #region UI Events

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForm())
            {
                isRegisterSucceed = false;
                m_IsToLoginProgress = true;
                pbProcessing.Visibility = Visibility.Visible;
                ITransition transition = FadeRollOut.GetTransition(SignInPanel);
                transition.Completed += new EventHandler(transition_Completed);
                transition.Begin();
                // AnimateHide(SignInPanel);
                btnNext.IsEnabled = false;
                btnCancel.IsEnabled = true;
            }
        }

        void transition_Completed(object sender, EventArgs e)
        {
            SignInPanel.Visibility = Visibility.Collapsed;
            //AnimateShow(SignInProgressPanel);
            SignInPanel.OpacityMask = null;
            SignInProgressPanel.Visibility = Visibility.Visible;
            ITransition transitionProressIn = FadeRollIn.GetTransition(SignInProgressPanel);
            transitionProressIn.Completed += new EventHandler(transitionProressIn_Completed);
            transitionProressIn.Begin();

        }

        void transitionProressIn_Completed(object sender, EventArgs e)
        {
            SignInProgressPanel.OpacityMask = null;
            Register(txtUsername.Text, txtPassword.Password);
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = false;
            m_IsToLoginProgress = false;
            btnNext.IsEnabled = true;
            ITransition transitionProgressOut = FadeRollOut.GetTransition(SignInProgressPanel);
            transitionProgressOut.Completed += new EventHandler(transitionProgressOut_Completed);
            transitionProgressOut.Begin();
            //AnimateHide(SignInProgressPanel);
        }

        void transitionProgressOut_Completed(object sender, EventArgs e)
        {
            SignInProgressPanel.Visibility = Visibility.Collapsed;
            SignInProgressPanel.OpacityMask = null;
            ITransition transitionSiginIn = FadeRollIn.GetTransition(SignInPanel);
            transitionSiginIn.Completed += new EventHandler(transitionSiginIn_Completed);
            SignInPanel.Visibility = Visibility.Visible;
            transitionSiginIn.Begin();
        }

        void transitionSiginIn_Completed(object sender, EventArgs e)
        {
            SignInPanel.OpacityMask = null;
        }
        #endregion


    }
}