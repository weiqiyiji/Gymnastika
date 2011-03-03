using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using Gymnastika.Common.Extensions;
using Gymnastika.Widgets;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets
{
    public class WidgetManager : IWidgetManager
    {
        private readonly IWidgetContainerAccessor _containerAccessor;
        private readonly IServiceLocator _serviceLocator;
          
        public WidgetManager(IWidgetContainerAccessor containerAccessor, IServiceLocator serviceLocator)
        {
            _containerAccessor = containerAccessor;
            _containerAccessor.ContainerReady += MonitoringWidgets;
            _serviceLocator = serviceLocator;
            Descriptors = new ObservableCollection<WidgetDescriptor>();
        }
        
        public ObservableCollection<WidgetDescriptor> Descriptors { get; private set; }

        public void Add(Type widgetType)
        {
            if (widgetType == null)
            {
                throw new ArgumentNullException("widgetType");
            }

            var descriptor = new WidgetDescriptor(widgetType);

            if(_containerAccessor.Container != null)
            {
                descriptor.IsActiveChanged += OnWidgetIsActiveChanged;
            }
            
            Descriptors.Add(descriptor);
        }

        private void MonitoringWidgets(object sender, EventArgs e)
        {
            foreach (var widgetDescriptor in Descriptors)
            {
                widgetDescriptor.IsActiveChanged += OnWidgetIsActiveChanged;
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

                if(widget == null)
                {
                    throw new InvalidOperationException(
                        string.Format("{0} doesn't exit", descriptor.WidgetType.FullName));
                }

                widgets.Remove(widget);
            }
        }

        public void Remove(Type widgetType)
        {
            if (widgetType == null)
            {
                throw new ArgumentNullException("widgetType");
            }

            var descriptor = Descriptors.SingleOrDefault(x => x.WidgetType == widgetType);

            if(descriptor == null)
            {
                throw new InvalidOperationException(string.Format("{0} doesn't exit", widgetType.FullName));
            }

            Descriptors.Remove(descriptor);
            descriptor.IsActive = false;
            descriptor.IsActiveChanged -= OnWidgetIsActiveChanged;
        }
    }
}
