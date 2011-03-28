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
using Gymnastika.Phone.Common;
namespace Gymnastika.Phone.Sync
{
    public class UserProfileService
    {
        public void LogOn(string username, string password)
        {
            HttpClient client = new HttpClient();
           client.PostCompeleted+=new EventHandler<HttpClient.PostCompeletedArgs>((s,e)=>
           {
           });
           using(MemoryStream ms=new MemoryStream())
           {
             ;
               client.Post(Config.GetServerPathUri(Config.LoginServiceUri),
                     DataContractHelper.ContractString( new LogOnInfo() { UserName = username, Password = password })
                   , null);
           }
        }
    }
}
