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
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
namespace Gymnastika.Phone.Sync
{
    public class Test
    {
        PushNotification.PushNotificationService service = new PushNotification.PushNotificationService();
        public void DoTest()
        {
            service.Connect();
            
        }
    }
}
