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
            public Stream ResponseStream { get; set; }
            public long ResponseLength { get; set; }
            public string Result { get; set; }
        }
        public event EventHandler<GetCompeletedArgs> GetCompeleted;
        public void Post(Uri uri, string xml, object UserToken)
        {
            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler((s, e) =>
            {
                if (PostCompeleted != null)
                {
                    PostCompeletedArgs arg = new PostCompeletedArgs() { UserToken = UserToken, Error = e.Error, Uri = uri };
                    if (e.Error == null && !e.Cancelled)
                    {
                        arg.Result = e.Result;
                    }
                    PostCompeleted(this, arg);
                }
            }
            );
            client.UploadStringAsync(uri, xml);

            //HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            //request.Method = "POST";
            //request.BeginGetRequestStream(new AsyncCallback((r) =>
            //{

            //    Stream requestStream = request.EndGetRequestStream(r);
            //    if (data != null)
            //    {
            //        requestStream.Write(data, 0, data.Length);
            //        requestStream.Flush();
            //    }
            //    request.BeginGetResponse(new AsyncCallback((r2) =>
            //    {
            //        HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(r2);
            //        if (PostCompeleted != null)
            //            PostCompeleted(this, new PostCompeletedArgs()
            //            {
            //                Uri = uri,
            //                UserToken = UserToken,
            //                ResponseLength = response.ContentLength,
            //                ResponseStream = response.GetResponseStream()
            //            });
            //    }
            //    ), null);
            //}
            //), null);
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
