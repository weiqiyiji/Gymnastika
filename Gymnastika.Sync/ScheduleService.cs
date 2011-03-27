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
using System.IO;
using System.Xml.Linq;
using Gymnastika.Data;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Sync
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ScheduleService
    {
        private RemindManager _remindManager;
        private IRepository<Models.ScheduleItem> _scheduleRepository;

        public ScheduleService(RemindManager remindManager, IRepository<Models.ScheduleItem> scheduleRepository)
        {
            _remindManager = remindManager;
            _scheduleRepository = scheduleRepository;
        }

        [WebInvoke(UriTemplate = "add", Method = "POST")]
        public TaskList Schedule(ScheduleItemCollection scheduleItems)
        {
            DateTime now = DateTime.Now;
            TaskList taskList = new TaskList();

            foreach (var scheduleItem in scheduleItems)
            {
                if (scheduleItem.StartTime < now)
                    throw new WebFaultException<string>(
                        string.Format("Schedule has wrong time range: startTime={0}, now={1}", scheduleItem.StartTime, now),
                        HttpStatusCode.BadRequest);

                int taskId = _remindManager.Add(scheduleItem);
                taskList.Add(new Task() { StartTime = scheduleItem.StartTime, TaskId = taskId, Message = scheduleItem.Message });
            }

            return taskList;
        }

        [WebGet(UriTemplate = "complete?id={id}")]
        public void CompleteTask(int id)
        {
            Models.ScheduleItem scheduleItem = _scheduleRepository.Get(id);

            if (scheduleItem == null)
            {
                throw new WebFaultException<string>(
                    string.Format("schedule {0} does not exist"),
                    HttpStatusCode.BadRequest);
            }

            scheduleItem.IsComplete = true;
            _scheduleRepository.Update(scheduleItem);
        }

        [WebGet(UriTemplate = "completed?user_id={userId}")]
        public TaskList GetCompletedTasks(int userId)
        {
            TaskList taskList = new TaskList();
            var completeTasks = _scheduleRepository.Fetch(x => x.UserId == userId && x.IsComplete == true);
            foreach (var schedule in completeTasks)
            {
                var startTime = DateTime.Parse(schedule.StartTime);

                if (IsToday(startTime))
                {
                    taskList.Add(new Task() { StartTime = startTime, TaskId = schedule.Id });
                }
            }

            return taskList;
        }

        private bool IsToday(DateTime date)
        {
            DateTime now = DateTime.Now;

            return date.Year == now.Year && date.Month == now.Month && date.Day == now.Day;
        }
    }
}