using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Http;
using System.Net;

namespace Gymnastika.Sync.Communication.Client
{
    public class RegistrationService
    {
        private const string RegisterBaseUri = "reg";
        private const string RegisterUri = "reg_desktop";
        private const string ConnectingUri = "connect?src_id={0}&desc_id={1}";
        private readonly string _baseAddress;

        public RegistrationService()
        {
            _baseAddress = new Uri(new Uri(Configuration.GetConfiguration("ServiceBaseUri")), RegisterBaseUri).AbsoluteUri;
        }

        public const int ResponseError = -1;

        public ResponseMessage Register()
        {
            using (HttpClient client = new HttpClient())
            {
                NetworkAdapterCollection networkAdapters = NetworkAdapterHelper.GetAdapters();

                HttpResponseMessage response = client.Post(
                    _baseAddress + "/" + RegisterUri,
                    HttpContentExtensions.CreateDataContract<NetworkAdapterCollection>(networkAdapters));

                return new ResponseMessage(response, HttpStatusCode.Created);
            }
        }

        public ResponseMessage Connect(string srcId, string targetId)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Get(
                    _baseAddress + "/" + string.Format(ConnectingUri, srcId, targetId));

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }
    }
}
