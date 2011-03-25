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
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Gymnastika.Phone.Sync
{
    public class ConnectionHelper
    {
        static Uri[] GetDesktopAccessPoints()
        {
            WebClient client = new WebClient();
            List<Uri> uris=new List<Uri>();
            AutoResetEvent autoReset=new AutoResetEvent(false);
            client.OpenReadCompleted
                += new OpenReadCompletedEventHandler((sender, e) => {
                    autoReset.Set();
                });
            client.OpenReadAsync(new Uri(Config.ServerUri, "/GetAddrs?username="), autoReset);
            autoReset.WaitOne();
            return uris.ToArray();
        }
    }
}
