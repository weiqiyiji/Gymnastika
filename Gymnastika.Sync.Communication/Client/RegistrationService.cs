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
        private const string RegisterUri = "reg_desktop";
        private readonly string _baseAddress;

        public RegistrationService()
        {
            _baseAddress = Configuration.GetConfiguration("RegistrationServiceBaseUri");
        }

        public const int ResponseError = -1;

        public ResponseMessage Register()
        {
            NetworkAdapterCollection networkAdapters = NetworkAdapterHelper.GetAdapters();
            ResponseMessage message = new ResponseMessage();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Post(
                    new Uri(new Uri(_baseAddress), RegisterUri),
                    HttpContentExtensions.CreateDataContract<NetworkAdapterCollection>(networkAdapters));

                message.StatusCode = response.StatusCode;

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    message.HasError = true;
                    return message;
                }

                string id = response.Content.ReadAsString();
                message.Result = id;
            }

            return message;
        }
    }
}
