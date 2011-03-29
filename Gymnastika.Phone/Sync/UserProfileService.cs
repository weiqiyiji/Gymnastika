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
using Gymnastika.Sync.Communication.Client;
namespace Gymnastika.Phone.Sync
{
    public class UserProfileService
    {
        public class LogOnCompeletedArgs : EventArgs
        {
            public string Id { get; set; }
            public bool Successful { get; set; }
            public string Message { get; set; }
        }
        public void LogOn(string username, string password, EventHandler<LogOnCompeletedArgs> callback)
        {
            HttpClient client = new HttpClient();
            client.PostCompeleted += new EventHandler<HttpClient.PostCompeletedArgs>((s, e) =>
            {
                if (e.Error == null)
                {
                    callback(this, new LogOnCompeletedArgs()
                    {
                        Successful = true,
                        Id = Util.GetXmlPureString(e.Result)
                    });
                }
                else
                    callback(this, new LogOnCompeletedArgs() { Successful = false,Message=e.Result });
            });
            using (MemoryStream ms = new MemoryStream())
            {
                client.Post(Config.GetServerPathUri(Config.LoginServiceUri),
                      DataContractHelper.ContractString(new LogOnInfo() { UserName = username, Password = password })
                    , null);
            }
        }
        public void LogOut(string Username)
        {
            HttpClient client = new HttpClient();
            client.PostCompeleted += new EventHandler<HttpClient.PostCompeletedArgs>((s, e) =>
            {
            });
            using (MemoryStream ms = new MemoryStream())
            {
                ;
                client.Get(Config.GetServerPathUri(string.Format(Config.LogoutSericeUri,HttpUtility.UrlEncode( Username))),
                     null);
            }
        }
    }
}
