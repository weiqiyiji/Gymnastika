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
        private readonly string _baseAddress;

        public RegistrationService()
        {
            _baseAddress = Configuration.GetConfiguration("ServiceBaseUri");
        }

        public const int ResponseError = -1;

        public ResponseMessage Register()
        {
            using (HttpClient client = new HttpClient())
            {
                NetworkAdapterCollection networkAdapters = NetworkAdapterHelper.GetAdapters();

                HttpResponseMessage response = client.Post(
                    new Uri(new Uri(_baseAddress), RegisterBaseUri + "/" + RegisterUri),
                    HttpContentExtensions.CreateDataContract<NetworkAdapterCollection>(networkAdapters));

                return new ResponseMessage(response, HttpStatusCode.Created);
            }
        }
    }
}
