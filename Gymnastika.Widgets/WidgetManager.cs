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
using Gymnastika.Widgets.Models;
using Gymnastika.Data;
using Gymnastika.Services.Session;

namespace Gymnastika.Widgets
{
    public class WidgetManager : IWidgetManager
    {
        private readonly IWidgetContainerAccessor _containerAccessor;
        private readonly IServiceLocator _serviceLocator;
        private readonly IRepository<WidgetInstance> _repository;
        private readonly ISessionManager _sessionManager;
          
        public WidgetManager(
            IWidgetContainerAccessor containerAccessor, 
            IServiceLocator serviceLocator, 
            IRepository<WidgetInstance> repository,
            ISessionManager sessionManager)
        {
            _containerAccessor = containerAccessor;
            _serviceLocator = serviceLocator;
            _repository = repository;
            _sessionManager = sessionManager;
            Descriptors = new ObservableCollection<WidgetDescriptor>();
        }
        
        public ObservableCollection<WidgetDescriptor> Descriptors { get; private set; }

        public void Add(Type widgetType)
        {
            if (widgetType == null)
            {
                throw new ArgumentNullException("widgetType");
            }

            //if(_containerAccessor.Container != null)
            //{
            //    descriptor.IsActiveChanged += OnWidgetIsActiveChanged;
            //}

            WidgetDescriptor descriptor = new WidgetDescriptor(widgetType);

            Descriptors.Add(descriptor);
        }

        //private void MonitoringWidgets(object sender, EventArgs e)
        //{
        //    foreach (var widgetDescriptor in Descriptors)
        //    {
        //        widgetDescriptor.IsActiveChanged += OnWidgetIsActiveChanged;
        //    }
        //}

        //private void OnWidgetIsActiveChanged(object sender, EventArgs e)
        //{
        //    WidgetDescriptor descriptor = (WidgetDescriptor)sender;
        //    var widgets = _containerAccessor.Container.Widgets;
        //    if (descriptor.IsActive)
        //    {
        //        IWidget widget = (IWidget)_serviceLocator.GetInstance(descriptor.WidgetType);
        //        widgets.Add(widget);
        //    }
        //    else
        //    {
        //        IWidget widget = widgets.SingleOrDefault(x => x.GetType() == descriptor.WidgetType);

        //        if(widget == null)
        //        {
        //            throw new InvalidOperationException(
        //                string.Format("{0} doesn't exit", descriptor.WidgetType.FullName));
        //        }

        //        widgets.Remove(widget);
        //    }
        //}

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
        }
    }
}
