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
      
        public class CheckAccessPointCompeletedArg:EventArgs
        {
            public DesktopAccessPoint AccessPoint { get;private set; }
            public bool Succussful { get; private set; }
            internal CheckAccessPointCompeletedArg(DesktopAccessPoint AccessPoint,bool Successful)
            {
                this.AccessPoint = AccessPoint;
                this.Succussful = Succussful;
            }
        }
        public event EventHandler<CheckAccessPointCompeletedArg> CheckAccessPointCompeleted;
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
        public void CheckAccessPoint(DesktopAccessPoint AccessPoint)
        {
            
        }
    }
}
