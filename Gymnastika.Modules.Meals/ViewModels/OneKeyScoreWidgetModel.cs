using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gymnastika.Modules.Meals.Communication.Services;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Sync.Communication;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Common.Events;
using Gymnastika.Common.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class OneKeyScoreWidgetModel
    {
        private readonly ISessionManager _sessionManager;
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly IEventAggregator _eventAggregator;
        private readonly User _user;
        private readonly CommunicationService _communicationService;

        public OneKeyScoreWidgetModel(CommunicationService communicationService,
            ISessionManager sessionManager,
            IFoodService foodService,
            IEventAggregator eventAggregator,
            IWorkEnvironment workEnvironment)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _eventAggregator = eventAggregator;
            _communicationService = communicationService;
            _sessionManager = sessionManager;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;


            _eventAggregator.GetEvent<RecieveOneKeyScoreEvent>().Subscribe(RecieveOneKeyScoreEventHandler);
        }

        public IList<TaskItem> TodayTaskItems { get; set; }

        private ICommand _scoreCommand;
        public ICommand ScoreCommand
        {
            get
            {
                if (_scoreCommand == null)
                    _scoreCommand = new DelegateCommand(Score);

                return _scoreCommand;
            }
        }

        public void Score()
        {
            _eventAggregator.GetEvent<SendOneKeyScoreEvent>().Publish(null);
            TodayTaskItems = new List<TaskItem>();
            _communicationService.GetCompletedTasks(_user.Id, OnGetCompletedTasksCallback);
        }

        private void RecieveOneKeyScoreEventHandler(List<TaskItem> taskItems)
        {
            TodayTaskItems.Union(taskItems);
        }

        private void OnGetCompletedTasksCallback(ResponseMessage response)
        {
            if (!response.HasError)
            {
                var taskList = response.Response.Content.ReadAsDataContract<TaskList>();

                using (var scope = _workEnvironment.GetWorkContextScope())
                {
                    foreach (var item in taskList)
                    {
                        DietPlanTask dietPlanTask = _foodService.DietPlanTaskProvider.Get(item.TaskId);
                        SubDietPlan subDietPlan = new SubDietPlan();
                        subDietPlan = _foodService.SubDietPlanProvider.Get(dietPlanTask.SubDietPlanId);
                        if (!subDietPlan.Mark)
                        {
                            subDietPlan.Mark = true;
                            _foodService.SubDietPlanProvider.Update(subDietPlan);
                        }
                    }
                    IEnumerable<DietPlanTask> dietPlanTasks = _foodService.DietPlanTaskProvider.GetDietPlanTasks(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    foreach (var item in dietPlanTasks)
                    {
                        SubDietPlan subDietPlan = new SubDietPlan();
                        subDietPlan = _foodService.SubDietPlanProvider.Get(item.SubDietPlanId);
                        TaskItem taskItem = new TaskItem()
                        {
                            Name = subDietPlan.MealName,
                            Score = subDietPlan.Score,
                            StartTime = subDietPlan.StartTime,
                            Mark = subDietPlan.Mark
                        };
                        TodayTaskItems.Add(taskItem);
                    }
                }
            }
        }
    }
}
