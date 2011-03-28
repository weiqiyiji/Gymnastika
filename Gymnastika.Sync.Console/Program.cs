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
using System.Runtime.Serialization;

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

            var profile = new UserProfileService();
            var test = profile.Test("test", "password");
            System.Console.WriteLine(test.Response.Content.ReadAsString());
            string targetid = System.Console.ReadLine();

            var registrationService = new RegistrationService();
            var registrationResponse = registrationService.Register();

            string srcId = PureString(registrationResponse.Response.Content.ReadAsString());

            var connectResponse = registrationService.Connect(srcId, targetid);
            string connectionId = PureString(connectResponse.Response.Content.ReadAsString());

            DateTime beginTime = DateTime.Now.AddSeconds(30);
            var scheduleItems = new ScheduleItemCollection()
                                 {
                                    new ScheduleItem()
                                    {
                                        UserId = 1,
                                        ConnectionId = int.Parse(connectionId),
                                        StartTime = beginTime,
                                        Message = "Hello"
                                    }
                                 };

            var scheduleService = new ScheduleService();
            var scheduleResponse = scheduleService.AddSchedule(scheduleItems);
            var taskList = scheduleResponse.Response.Content.ReadAsDataContract<TaskList>();

            foreach(var task in taskList)
            {
                scheduleService.CompleteTask(task.TaskId);
            }

            taskList = scheduleService.GetCompletedTasks(1).Response.Content.ReadAsDataContract<TaskList>();

            taskList = scheduleService.GetTodayPlans(1).Response.Content.ReadAsDataContract<TaskList>();
        }
    }
}
