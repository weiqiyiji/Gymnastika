using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Sync.Communication;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Http;
using Gymnastika.Sync.Communication.Client;
using System.Xml.Linq;

namespace Gymnastika.Sync.Console
{

    class Program
    {
        private const string RegisterUri = "reg_desktop";
        private const string ConnectUri = "connect?src_id={0}&desc_id={1}";
        private const string _baseAddress = "http://localhost/gym";

        static string PureString(string input)
        {
            XElement element = XElement.Parse(input);
            return element.Value;
        }

        static void Main(string[] args)
        {
            string targetid = System.Console.ReadLine();

            NetworkAdapterCollection networkAdapters = NetworkAdapterHelper.GetAdapters();

            HttpClient client = new HttpClient(_baseAddress);
            HttpResponseMessage response = client.Post(
                _baseAddress + "/reg/" + RegisterUri,
                HttpContentExtensions.CreateDataContract<NetworkAdapterCollection>(networkAdapters));

            string id = PureString(response.Content.ReadAsString());

            client = new HttpClient();
            response = client.Get(_baseAddress + "/reg/" + string.Format(ConnectUri, id, targetid));
            string connectionId = PureString(response.Content.ReadAsString());
            Plan plan = new Plan();
            DateTime beginTime = DateTime.Now.AddSeconds(10);
            plan.ScheduleItems = new ScheduleItemCollection()
                                 {
                                    new ScheduleItem()
                                    {
                                        ConnectionId = int.Parse(connectionId),
                                        StartTime = beginTime,
                                        EndTime = beginTime,
                                        Message = "Hello"
                                    }
                                 };
            response = client.Post(
                _baseAddress + "/schedule/add",
                HttpContentExtensions.CreateDataContract<Plan>(plan));
        }
    }
}
