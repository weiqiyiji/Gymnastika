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
using Gymnastika.Phone.Common;

namespace Gymnastika.Phone.Sync
{
    public class PlanSync
    {
     
        public delegate void CompeleteTaskCallback(int id,bool successful);
        public static void CompeleteTask(int Id,CompeleteTaskCallback callback)
        {
            HttpClient client = new HttpClient();
            client.GetCompeleted+=new EventHandler<HttpClient.GetCompeletedArgs>((s,e)=>
            {
                if (callback != null)
                {
                    callback(Id, e.Error == null);
                }
            });
            client.Get(Config.GetServerPathUri(string.Format(Config.CompeleteTaskUri, Id)), null);
        }
    }
}