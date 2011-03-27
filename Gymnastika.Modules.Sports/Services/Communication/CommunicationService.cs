using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Sync.Communication;
using Gymnastika.Modules.Sports.Facilities;

namespace Gymnastika.Modules.Sports.Services.Communication
{
    public class CommunicationService
    {
        public void SendPlan(SportsPlan plan, int connectionId ,Action<ResponseMessage> callback)
        {
            var scheduleItems = new ScheduleItemCollection();
            foreach (var item in plan.SportsPlanItems)
            {
                var scheduleItem = new ScheduleItem();
                scheduleItem.UserId = plan.User.Id;
                scheduleItem.ConnectionId = connectionId;
                scheduleItem.StartTime = new DateTime(plan.Year, plan.Month, plan.Day, item.Hour, item.Minute, 0);
                scheduleItem.Message = PlanFormatter.Format(item);
                scheduleItems.Add(scheduleItem);
            }

            ScheduleService service = new ScheduleService();
            AsychronousLoadHelper.AsychronousCall(() =>
                {
                    var response = service.AddSchedule(scheduleItems);
                    callback(response);
                });
        }
    }
}
