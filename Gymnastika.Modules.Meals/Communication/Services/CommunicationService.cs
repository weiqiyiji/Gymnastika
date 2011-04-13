using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Sync.Communication;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Modules.Meals.Communication.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Gymnastika.Modules.Meals.Helpers;
using System.Collections;

namespace Gymnastika.Modules.Meals.Communication.Services
{
    public class CommunicationService
    {
        private readonly ConnectionStore _connectionStore;

        public CommunicationService(ConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }

        public void SendTasks(DietPlan dietPlan, Action<ResponseMessage, DietPlan> callback)
        {
            if (_connectionStore.IsConnectionEstablished)
            {
                TransferModelToTaskItem(dietPlan);

                var scheduleItems = new ScheduleItemCollection();

                foreach (var taskItem in TodayTasks)
                {
                    var scheduleItem = new ScheduleItem()
                    {
                        UserId = dietPlan.User.Id,
                        ConnectionId = _connectionStore.ConnectionId,
                        StartTime = taskItem.StartTime,
                        Message = TransferTaskItemToXml(taskItem)
                    };
                    scheduleItems.Add(scheduleItem);
                }

                AsychronousLoadHelper.AsychronousCall(() =>
                {
                    var scheduleService = new ScheduleService();
                    var response = scheduleService.AddSchedule(scheduleItems);
                    callback(response, dietPlan);
                });
            }
        }

        public void GetCompletedTasks(int userId, Action<ResponseMessage> callback)
        {
            AsychronousLoadHelper.AsychronousCall(() =>
            {
                var scheduleService = new ScheduleService();
                var response = scheduleService.GetCompletedTasks(userId);
                callback(response);
            });
        }

        public List<DietPlanTaskItem> TodayTasks { get; set; }

        private void TransferModelToTaskItem(DietPlan dietPlan)
        {
            TodayTasks = new List<DietPlanTaskItem>();

            foreach (var subDietPlan in dietPlan.SubDietPlans)
            {
                DietPlanTaskItem dietPlanTaskItem = new DietPlanTaskItem()
                   {
                       Id = subDietPlan.Id,
                       Score = subDietPlan.Score,
                       StartTime = subDietPlan.StartTime
                   };
                FoodTaskList foodTasks = new FoodTaskList();
                //FoodTaskItem[] foodTasks = new FoodTaskItem[] { };
                foreach (var dietPlanItem in subDietPlan.DietPlanItems)
                {
                    FoodTaskItem foodTaskItem = new FoodTaskItem()
                    {
                        Id = dietPlanItem.Id,
                        Amount = (int)dietPlanItem.Amount,
                        Calorie = (int)dietPlanItem.Food.Calorie,
                        FoodName = dietPlanItem.Food.Name
                    };
                    foodTasks.Add(foodTaskItem);
                }
                dietPlanTaskItem.FoodTasks = foodTasks;
                TodayTasks.Add(dietPlanTaskItem);
            }
        }

        private string TransferTaskItemToXml(object obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
