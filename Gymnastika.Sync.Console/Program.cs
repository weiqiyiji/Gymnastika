using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Sync.Communication;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Http;
using Gymnastika.Sync.Communication.Client;

namespace Gymnastika.Sync.Console
{
    class Program
    {
        private const string RegisterUri = "reg_desktop";
        private const string _baseAddress = "http://localhost/gym";

        static void Main(string[] args)
        {
            NetworkAdapterCollection networkAdapters = NetworkAdapterHelper.GetAdapters();

            HttpClient client = new HttpClient(_baseAddress);
            HttpResponseMessage response = client.Post(
                _baseAddress + "/" + RegisterUri,
                HttpContentExtensions.CreateDataContract<NetworkAdapterCollection>(networkAdapters));

            string id = response.Content.ReadAsString();

            client = new HttpClient();

        }
    }
}
