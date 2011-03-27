using System;
using System.Windows;
using System.Windows.Controls;
using Gymnastika.Widgets.Infrastructure;
using System.Windows.Threading;
using System.Timers;
using System.Windows.Media;

namespace Gymnastika.Widgets
{
    /// <summary>
    /// Interaction logic for DateWidget.xaml
    /// </summary>
    [WidgetMetadata("时间", "/Gymnastika.Infrastructure;component/Images/datewidget_icon.png")]
    public partial class DateWidget : UserControl, IWidget, IDisposable
    {
        private Timer _timer;
        private Timer _secondTimer;
        private const double Angle = 6;

        public DateWidget()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            UpdateTime();
            DateTime now = DateTime.Now;

            UpdateSecond();

            _secondTimer = new Timer();
            _secondTimer.AutoReset = true;
            _secondTimer.Elapsed += new ElapsedEventHandler(_secondTimer_Elapsed);
            _secondTimer.Interval = 1000;
            _secondTimer.Start();

            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = (60 - now.Second) * 1000 + (1000 - now.Millisecond);
            
            timer.Start();
        }

        private void UpdateSecond()
        {
            RotateTransform transform = (RotateTransform)this.second.RenderTransform;
            transform.Angle = DateTime.Now.Second * Angle;
        }

        private void _secondTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)UpdateSecond);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)UpdateTime);
            Timer oldTimer = (Timer)sender;
            oldTimer.Elapsed -= timer_Elapsed;
            oldTimer.Stop();

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)UpdateTime);
        }

        private void UpdateTime()
        {
            this.dateLabel.Text = DateTime.Now.ToString("HH : mm");
        }

        #region IDisposable Members

        public void Dispose()
        {
            _timer.Stop();
            _timer.Elapsed -= _timer_Elapsed;

            _secondTimer.Stop();
            _secondTimer.Elapsed -= _secondTimer_Elapsed;
        }

        #endregion
    }
}
