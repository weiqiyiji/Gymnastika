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
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace Gymnastika.Phone.PushNotification
{
    public class SubscribeCompletedEventArgs : EventArgs
    {
        public int AssignedId { get; set; }

        public SubscribeCompletedEventArgs(int id)
        {
            AssignedId = id;
        }
    }

    public class PushNotificationService
    {
        private HttpNotificationChannel _httpChannel;
        private const string ChannelName = "GymnastikaSyncChannel";
        private const string FileName = "PushNotificationsSettings.dat";

        public event EventHandler<NotificationChannelErrorEventArgs> ErrorOccurred;
        public event EventHandler<HttpNotificationEventArgs> HttpNotificationReceived;
        public event EventHandler<SubscribeCompletedEventArgs> SubscribeCompleted;

        public int AssignedId { get; set; }

        private void Trace(string message)
        {
#if DEBUG
            Debug.WriteLine(message);
#endif
        }

        public void Connect()
        {
            if (!TryFindChannel())
                DoConnect();
        }

        private void DoConnect()
        {
            try
            {
                //First, try to pick up existing channel
                _httpChannel = HttpNotificationChannel.Find(ChannelName);

                if (null != _httpChannel)
                {
                    Trace("Channel Exists - no need to create a new one");
                    SubscribeToChannelEvents();

                    Trace("Register the URI with 3rd party web service");
                    SubscribeToService();

                    //TODO: Place Notification

                    Trace("Channel recovered");
                }
                else
                {
                    Trace("Trying to create a new channel...");
                    //Create the channel
                    _httpChannel = new HttpNotificationChannel(ChannelName);
                    Trace("New Push Notification channel created successfully");

                    SubscribeToChannelEvents();

                    Trace("Trying to open the channel");
                    _httpChannel.Open();
                    Trace("Channel open requested");
                }
            }
            catch (Exception ex)
            {
                Trace("Channel error: " + ex.Message);
            }
        }

        private bool TryFindChannel()
        {
            bool bRes = false;

            Trace("Getting IsolatedStorage for current Application");
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Trace("Checking channel data");
                if (isf.FileExists(FileName))
                {
                    Trace("Channel data exists! Loading...");
                    using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(FileName, FileMode.Open, isf))
                    {
                        using (StreamReader sr = new StreamReader(isfs))
                        {
                            string uri = sr.ReadLine();
                            Trace("Finding channel");
                            _httpChannel = HttpNotificationChannel.Find(ChannelName);

                            if (null != _httpChannel)
                            {
                                if (_httpChannel.ChannelUri.ToString() == uri)
                                {
                                    SubscribeToChannelEvents();
                                    SubscribeToService();
                                    bRes = true;
                                }

                                sr.Close();
                            }
                        }
                    }
                }
                else
                    Trace("Channel data not found");
            }

            return bRes;
        }

        private void SubscribeToChannelEvents()
        {
            //Register to UriUpdated event - occurs when channel successfully opens
            _httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(OnChannelUriUpdated);

            //Subscribed to Raw Notification
            _httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(OnHttpNotificationReceived);

            //general error handling for push channel
            _httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(OnExceptionOccurred);
        }

        private void OnChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Trace("Channel opened. Got Uri:\n" + _httpChannel.ChannelUri.ToString());
            SaveChannelInfo();

            Trace("Subscribing to channel events");
            SubscribeToService();
        }

        private void SubscribeToService()
        {
            string theUri = String.Format("http://localhost/gym/reg/reg_phone?uri={0}", _httpChannel.ChannelUri.ToString());
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, e) =>
            {
                if (null == e.Error)
                {
                    Trace("Registration succeeded");
                    XElement result = XElement.Parse(e.Result);
                    AssignedId = int.Parse(result.Value);
                    RaiseSubscribeCompleted(AssignedId);
                }
                else
                    Trace("Registration failed: " + e.Error.Message);
            };
            client.DownloadStringAsync(new Uri(theUri));
        }

        private void RaiseSubscribeCompleted(int id)
        {
            if (SubscribeCompleted != null)
                SubscribeCompleted(this, new SubscribeCompletedEventArgs(id));
        }

        private void OnExceptionOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            if (ErrorOccurred != null)
            {
                ErrorOccurred(sender, e);
            }
        }

        private void OnHttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            Trace("===============================================");
            Trace("RAW notification arrived:");

            if (HttpNotificationReceived != null)
                HttpNotificationReceived(sender, e);

            Trace("===============================================");
        }

        private void SaveChannelInfo()
        {
            Trace("Getting IsolatedStorage for current Application");
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Trace("Creating data file");
                using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(FileName, FileMode.Create, isf))
                {
                    using (StreamWriter sw = new StreamWriter(isfs))
                    {
                        Trace("Saving channel data...");
                        sw.WriteLine(_httpChannel.ChannelUri.ToString());
                        sw.Close();
                        Trace("Saving done");
                    }
                }
            }
        }
    }
}
