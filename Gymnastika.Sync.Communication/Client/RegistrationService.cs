using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Http;

namespace Gymnastika.Sync.Communication.Client
{
    public class RegistrationService
    {
        private const string RegisterUri = "reg_desktop";
        private readonly string _baseAddress;

        public RegistrationService()
        {
            _baseAddress = Configuration.GetConfiguration("SyncServiceBaseUri");
        }

        public int Register()
        {
            NetworkAdapterCollection networkAdapters = NetworkAdapterHelper.GetAdapters();
            int assigndId;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Post(
                    new Uri(new Uri(_baseAddress), RegisterUri),
                    HttpContentExtensions.CreateDataContract<NetworkAdapterCollection>(networkAdapters));

                string id = response.Content.ReadAsString();
                assigndId = int.Parse(id);
            }

            return assigndId;
        }
    }
}
