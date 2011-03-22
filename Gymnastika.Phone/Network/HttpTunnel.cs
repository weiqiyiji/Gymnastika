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

namespace Gymnastika.Phone.Network
{
    public class HttpTunnel
    {
        Uri m_host;

        #region helpers
        class InvokeState
        {
            public object Caller;
            public object[] AdditionalData;
            public InvokeState(object Caller)
            {
                this.Caller = Caller;
            }
            public InvokeState(object Caller, params object[] AdditionalData)
                :this(Caller)
            {
                this.AdditionalData = AdditionalData;
            }
        }
        HttpWebRequest CreateReuqest(string path)
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(m_host, path));
            return request;
        }
        HttpWebRequest CreateReuqest(string pathFormat, params object[] values)
        {
            string pathAndQuery = pathFormat;
            switch (values.Length)
            {
                case 1:
                    pathAndQuery = string.Format(pathAndQuery, values[0]);
                    break;
                case 2:
                    pathAndQuery = string.Format(pathAndQuery, values[0], values[1]);
                    break;
                case 3:
                    pathAndQuery = string.Format(pathAndQuery, values[0], values[1], values[2]);
                    break;
                case 4:
                    pathAndQuery = string.Format(pathAndQuery, values[0], values[1], values[2], values[3]);
                    break;
                case 5:
                    pathAndQuery = string.Format(pathAndQuery, values[0], values[1], values[2], values[3], values[4]);
                    break;
                case 6:
                    pathAndQuery = string.Format(pathAndQuery, values[0], values[1], values[2], values[3], values[4], values[5]);
                    break;
                case 7:
                    pathAndQuery = string.Format(pathAndQuery, values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
                    break;
            }
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(m_host, pathAndQuery));
            return request;
        }
        HttpWebResponse EndGetResponse(IAsyncResult result, out HttpWebRequest request)
        {
            request = result.AsyncState as HttpWebRequest;
            return request.EndGetResponse(result) as HttpWebResponse;
        }
        HttpWebResponse EndGetResponse(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            return request.EndGetResponse(result) as HttpWebResponse;
        }
        #endregion

        public delegate void ActionCompeletedHandler(object sender, HttpWebRequest request, HttpWebResponse response);
        public event ActionCompeletedHandler ActionCompeleted;

        public HttpTunnel(Uri host)
        {
            this.m_host = host;
        }

        public void ConnectToPC(string PCHost, int PCPort)
        {
            m_host = new Uri(
                string.Format("http://{0}:{1}", PCHost, PCPort)
                );
            HttpWebRequest request = CreateReuqest("regsiter.do?username="
                + HttpUtility.UrlEncode(UserProfile.UserProfileManager.ActiveProfile.Username));
            request.BeginGetResponse(new AsyncCallback(ConnectCompeleted), request);
        }

        private void ConnectCompeleted(IAsyncResult result)
        {
            HttpWebResponse response = EndGetResponse(result);
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string connectResult = reader.ReadToEnd();

            }
        }

        private void GetResponseCallback(IAsyncResult result)
        {
            HttpWebRequest request;
            HttpWebResponse response = EndGetResponse(result,out request);
            if (ActionCompeleted != null)
            {
                ActionCompeleted.BeginInvoke(this, request, response, new AsyncCallback(
                    (r) =>
                    {
                        response.Dispose();
                    }), request);
            }
            else
                response.Dispose();
        }

        public IAsyncResult DoAction(string action, string[] names, string[] values)
        {
            string query = string.Empty;
            if (names != null && values != null)
            {
                if (names.Length != values.Length)
                {
                    throw new ArgumentException("Length of names doesn't match that of values");
                }
                for (int i = 0; i < names.Length; ++i)
                {
                    query += string.Format("&{0}={1}", names[i], HttpUtility.UrlEncode(values[i]));
                }
            }
            HttpWebRequest request = CreateReuqest("action.do?act={0}{1}", HttpUtility.UrlEncode(action), query);
            return request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }
    }
}
