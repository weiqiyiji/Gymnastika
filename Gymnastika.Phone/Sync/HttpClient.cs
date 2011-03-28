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
using System.Text;

namespace Gymnastika.Phone.Sync
{
    public class HttpClient
    {
        public class PostCompeletedArgs : EventArgs
        {
            public Uri Uri { get; set; }
            public Exception Error { get; set; }
            public object UserToken { get; set; }
            public string Result { get; set; }
        }
        public event EventHandler<PostCompeletedArgs> PostCompeleted;
        public class GetCompeletedArgs : EventArgs
        {
            public Uri Uri { get; set; }
            public Exception Error { get; set; }
            public object UserToken { get; set; }
            public string Result { get; set; }
        }
        public event EventHandler<GetCompeletedArgs> GetCompeleted;
        public void Post(Uri uri, string xml, object UserToken)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.BeginGetRequestStream(new AsyncCallback((r) =>
            {

                Stream requestStream = request.EndGetRequestStream(r);
                if (xml != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(xml);
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }
                request.BeginGetResponse(new AsyncCallback((r2) =>
                {
                    Exception ex = null;
                     HttpWebResponse response =null;
                
                     try
                     {
                         response = (HttpWebResponse)request.EndGetResponse(r2);
                     }
                     catch (Exception e)
                     {
                         ex = e;
                     }
                    if (PostCompeleted != null)
                    {
                        PostCompeletedArgs arg = new PostCompeletedArgs()
                        {
                            Error = ex,
                            Uri = uri,
                            UserToken = UserToken
                        };
                        if (ex == null)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                arg.Result = reader.ReadToEnd();
                            }
                        }
                        PostCompeleted(this, arg);
                    }
                }
                ), null);
            }
            ), null);
        }
        public void Get(Uri uri, object UserToken)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler((s, e) =>
            {
                if (GetCompeleted != null)
                {
                    GetCompeletedArgs arg = new GetCompeletedArgs() { UserToken = UserToken, Error = e.Error, Uri = uri };
                    if (e.Error == null && !e.Cancelled)
                    {
                        arg.Result = e.Result;
                    }
                    GetCompeleted(this, arg);
                }
            }
            );
            client.DownloadStringAsync(uri);

        }
    }
}
