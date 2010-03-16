using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Gymnastika.Widgets.Models;
using Gymnastika.Data;
using Gymnastika.Services.Session;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Services.Models;

namespace Gymnastika.Widgets.Behaviors
{
    public class SaveWidgetStateBehavior : WidgetContainerBehaviorBase
    {
        public const string BehaviorKey = "SaveWidgetStateBehavior";

        private readonly IWidgetManager _widgetManager;
        private readonly ISessionManager _sessionManager;
        private readonly IRepository<WidgetInstance> _widgetRepository;
        private readonly IRepository<User> _userRepository;

        public SaveWidgetStateBehavior(
            IWidgetManager widgetManager,
            ISessionManager sessionManager,
            IRepository<WidgetInstance> widgetRepository,
            IRepository<User> userRepository)
        {
            _widgetManager = widgetManager;
            _sessionManager = sessionManager;
            _widgetRepository = widgetRepository;
            _userRepository = userRepository;
        }

        protected override void OnAttach()
        {
            Application.Current.Exit += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            SaveState();
        }

        private void SaveState()
        {
            User user = _sessionManager.GetCurrentSession().AssociatedUser;

            using(IWorkContextScope scope = ServiceLocator.Current.GetInstance<IWorkEnvironment>().GetWorkContextScope())
            {
                IList<WidgetInstance> widgetInstances = _widgetRepository.Fetch(x => true).ToList();
                user = _userRepository.Get(user.Id);

                foreach (IWidgetHost host in this.Target.WidgetHosts)
                {
                    var descriptor = _widgetManager.Descriptors.Single(x => x.WidgetType == host.Widget.GetType());                   
                    WidgetInstance instance = widgetInstances.SingleOrDefault(x => x.User.Id == user.Id && x.Type == descriptor.WidgetType.FullName);

                    if (instance == null)
                    {
                        instance = new WidgetInstance
                        {
                            Type = descriptor.WidgetType.FullName,
                            DisplayName = descriptor.DisplayName,
                            Icon = descriptor.Icon,
                            X = descriptor.Position.X,
                            Y = descriptor.Position.Y,
                            IsActive = descriptor.IsActive,
                            User = user
                        };

                        _widgetRepository.Create(instance);
                    }
                    else
                    {
                        widgetInstances.Remove(instance);
                        instance.X = descriptor.Position.X;
                        instance.Y = descriptor.Position.Y;
                        _widgetRepository.Update(instance);
                    }
                }

                foreach (WidgetInstance instance in widgetInstances)
                {
                    _widgetRepository.Delete(instance);
                }
            }
        }
    }
}
