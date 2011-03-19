using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Gymnastika.Sync.Models;
using Gymnastika.Sync.Communication;
using System.Net;
using Gymnastika.Sync.Schedule;

namespace Gymnastika.Sync
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ScheduleService
    {
        private RemindManager _remindManager;

        public ScheduleService(RemindManager remindManager)
        {
            _remindManager = remindManager;
        }

        [WebInvoke(UriTemplate = "", Method = "POST")]
        public void Schedule(Plan plan)
        {
            DateTime now = DateTime.Now;

            foreach (var scheduleItem in plan.ScheduleItems)
            {
                if(scheduleItem.StartTime > scheduleItem.EndTime)
                    throw new WebFaultException<string>(
                        string.Format("Schedule has wrong time range: startTime={0}, endTime={1}", scheduleItem.StartTime, scheduleItem.EndTime),
                        HttpStatusCode.BadRequest);

                if (scheduleItem.EndTime < now)
                {
                    _remindManager.Add(scheduleItem);
                }
            }
        }
    }
}