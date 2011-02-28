using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Gymnastika.Widgets.Behaviors
{
    public class AutoRegisterWidgetBehavior : IWidgetContainerBehavior
    {
        private readonly IUnityContainer _container;
        private readonly IWidgetManager _widgetManager;

        public AutoRegisterWidgetBehavior(IUnityContainer container, IWidgetManager widgetManager)
        {
            _container = container;
            _widgetManager = widgetManager;
        }

        public void Attach()
        {
            _widgetManager.Descriptors.CollectionChanged += OnDescriptorsCollectionChanged;

            foreach(var descriptor in _widgetManager.Descriptors)
            {
                _container.RegisterType(descriptor.WidgetType);
            }
        }

        private void OnDescriptorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (WidgetDescriptor descriptor in e.NewItems)
                {
                    _container.RegisterType(descriptor.WidgetType);
                }
            }
        }

        public IWidgetContainer Target { get; set; }
    }
}
