using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using System.Net;

namespace Gymnastika.Sync.Communication.Client
{
    public class ScheduleService
    {
        private const string ScheduleBaseUri = "schedule";
        private const string AddPlanUri = "/add";
        private readonly string _baseAddress;

        public ScheduleService()
        {
            _baseAddress = new Uri(new Uri(Configuration.GetConfiguration("ServiceBaseUri")), ScheduleBaseUri).AbsoluteUri;
        }

        public ResponseMessage AddSchedule(ScheduleItemCollection scheduleItems)
        {
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Post(
                    _baseAddress + AddPlanUri, HttpContentExtensions.CreateDataContract<ScheduleItemCollection>(scheduleItems));

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }

        public ResponseMessage CompleteTask(int taskId)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Get(
                    _baseAddress + "/complete?id=" + taskId.ToString());

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }

        public ResponseMessage GetCompletedTasks(int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Get(
                    _baseAddress + "completed?user_id=" + userId);

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }
    }
}
