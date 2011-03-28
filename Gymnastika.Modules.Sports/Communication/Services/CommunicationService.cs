using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Sync.Communication;
using Gymnastika.Modules.Sports.Facilities;
using System.Runtime.Serialization;
using Gymnastika.Modules.Sports.Communication.Tasks;
using System.IO;
using System.Xml;
using System.Windows.Threading;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Sports.Communication.Helper;

namespace Gymnastika.Modules.Sports.Communication.Services
{
    public class CommunicationService
    {

        public void SendPlan(SportsPlan plan, int connectionId, Action<ResponseMessage, SportsPlan> callback)
        {
            var scheduleItems = new ScheduleItemCollection();
            foreach (var item in plan.SportsPlanItems)
            {
                var scheduleItem = new ScheduleItem();
                scheduleItem.UserId = plan.User.Id;
                scheduleItem.ConnectionId = connectionId;
                scheduleItem.StartTime = new DateTime(plan.Year, plan.Month, plan.Day, item.Hour, item.Minute, 0);
                DataContractSerializer sr = new DataContractSerializer(typeof(SportsPlanTaskItem));
                MemoryStream stream = new MemoryStream();

                SportsPlanTaskItem taskItem = new SportsPlanTaskItem()
                {
                    Calories = item.Sport.Calories,
                    Minutes = item.Sport.Minutes,
                    Duration = item.Duration,
                    Time = scheduleItem.StartTime,
                    SportName = item.Sport.Name,
                    Id = item.Id
                };

                scheduleItem.Message = ContractObjectSerializer.Serialize(taskItem);
                scheduleItems.Add(scheduleItem);
            }

            ScheduleService service = new ScheduleService();
            AsychronousLoadHelper.AsychronousCall(() =>
                {
                    var response = service.AddSchedule(scheduleItems);
                    callback(response, plan);
                });

        }

        public IList<SportsPlanTaskItem> GetCompletedTasks(int userId)
        {
            ScheduleService service = new ScheduleService();
            var response = service.GetCompletedTasks(userId);
            if (response.HasError)
                throw new Exception(response.ErrorMessage);
            var tasks = response.Response.Content.ReadAsDataContract<TaskList>();
            IList<SportsPlanTaskItem> items = tasks.Select
                (s => ContractObjectSerializer
                    .Deserialize<SportsPlanTaskItem>(s.Message))
                    .ToList();
            return items;
        }
    }
}
