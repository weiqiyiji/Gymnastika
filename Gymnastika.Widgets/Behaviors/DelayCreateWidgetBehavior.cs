using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Gymnastika.Widgets.Models;
using Gymnastika.Data;
using Gymnastika.Services.Session;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Services.Models;
using System.Collections;

namespace Gymnastika.Widgets.Behaviors
{
    public class DelayCreateWidgetBehavior : WidgetContainerBehaviorBase
    {
        public const string BehaviorKey = "DelayCreateWidgetBehavior";

        private readonly IWidgetManager _widgetManager;
        private readonly IWidgetContainerAccessor _containerAccessor;
        private readonly ISessionManager _sessionManager;
        private readonly IServiceLocator _serviceLocator;
        private readonly IRepository<WidgetInstance> _widgetRepository;
        private IList<WidgetInstance> _widgetInstances;

        public DelayCreateWidgetBehavior(
            IWidgetManager widgetManager, 
            IWidgetContainerAccessor containerAccessor,
            ISessionManager sessionManager,
            IServiceLocator serviceLocator,
            IRepository<WidgetInstance> widgetRepository)
        {
            _widgetManager = widgetManager;
            _containerAccessor = containerAccessor;
            _sessionManager = sessionManager;
            _serviceLocator = serviceLocator;
            _widgetRepository = widgetRepository;
        }

        protected override void OnAttach()
        {
            if (_sessionManager.GetCurrentSession() != null)
            {
                User user = _sessionManager.GetCurrentSession().AssociatedUser;

                using (IWorkContextScope scope = ServiceLocator.Current.GetInstance<IWorkEnvironment>().GetWorkContextScope())
                {
                    _widgetInstances = _widgetRepository.Fetch(x => x.User.Id == user.Id).ToList();
                }

                StartCreateWidgets(_widgetManager.Descriptors);

                _widgetManager.Descriptors.CollectionChanged += OnDescriptorCollectionChanged;
            }
        }

        private void OnWidgetIsActiveChanged(object sender, EventArgs e)
        {
            WidgetDescriptor descriptor = (WidgetDescriptor)sender;

            var widgets = _containerAccessor.Container.Widgets;

            if (descriptor.IsActive)
            {
                IWidget widget = (IWidget)_serviceLocator.GetInstance(descriptor.WidgetType);
                widgets.Add(widget);
            }
            else
            {
                IWidget widget = widgets.SingleOrDefault(x => x.GetType() == descriptor.WidgetType);

                if (widget == null)
                {
                    throw new InvalidOperationException(
                        string.Format("{0} doesn't exit", descriptor.WidgetType.FullName));
                }

                widgets.Remove(widget);
            }
        }

        private void OnDescriptorCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                StartCreateWidgets(e.NewItems);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (WidgetDescriptor descriptor in e.OldItems)
                {
                    descriptor.IsActive = false;
                    descriptor.IsActiveChanged -= OnWidgetIsActiveChanged;
                }
            }
        }

        private void StartCreateWidgets(IList widgets)
        {
            foreach (WidgetDescriptor descriptor in widgets)
            {
                descriptor.IsActiveChanged += OnWidgetIsActiveChanged;

                WidgetInstance instance = _widgetInstances.SingleOrDefault(
                    x => x.Type == descriptor.WidgetType.FullName);

                if (instance != null)
                {
                    descriptor.Initialize(instance);
                }
                else
                {
                    descriptor.Initialize();
                }
            }
        }
    }
}
