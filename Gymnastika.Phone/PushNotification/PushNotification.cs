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
using Microsoft.Phone.Notification;
namespace Gymnastika.Phone.PushNotification
{
    public static class PushNotification
    {
        private static HttpNotificationChannel httpChannel;
        private static string channelName = "GymnastikaChannel";
        private static string channelServiceName = "GymnastikaService";
        public static void DoConnect()
        {
            httpChannel = HttpNotificationChannel.Find(channelName);
            if (httpChannel != null)
            {
                SubscribeToChannelEvents();
                SubscribeToService();
                SubscribeToNotifications();
            }
            else
            {
                httpChannel = new HttpNotificationChannel(channelName, channelServiceName);
                SubscribeToChannelEvents();
                httpChannel.Open();
            }
        }
        private static void SubscribeToChannelEvents()
        {
            httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);
            httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);
            httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
        }

        private static void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            
        }

        private static void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            
        }

        private static void httpChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            
        }
        private static void SubscribeToService()
        {

        }
        private static void SubscribeToNotifications()
        {

        }
    }
}
