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
using Gymnastika.Phone.PushNotification;
using System.IO;

namespace Gymnastika.Phone.ConnectionSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PushNotificationService _service;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _service = new PushNotificationService();
            _service.SubscribeCompleted += new EventHandler<SubscribeCompletedEventArgs>(_service_SubscribeCompleted);
            _service.HttpNotificationReceived += new EventHandler<Microsoft.Phone.Notification.HttpNotificationEventArgs>(_service_HttpNotificationReceived);
            _service.Connect();
        }

        void _service_SubscribeCompleted(object sender, SubscribeCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => ApplicationTitle.Text = e.AssignedId.ToString());
        }

        void _service_HttpNotificationReceived(object sender, Microsoft.Phone.Notification.HttpNotificationEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    using (StreamReader reader = new StreamReader(e.Notification.Body))
                    {
                        ApplicationTitle.Text = reader.ReadToEnd();
                    }
                });
        }
    }
}