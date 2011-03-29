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

namespace Gymnastika.Phone.Sync
{
    public class Connection
    {
        public void Close()
        {

        }
        private delegate bool ConnectDelegate(DesktopAccessPoint AccessPoint, ConnectDelegate _Delegate);


        public void ConnectAsync(DesktopAccessPoint AccessPoint)
        {
            WebClient client = new WebClient();
         //   client.DownloadStringAsync()
        }
    }
}
