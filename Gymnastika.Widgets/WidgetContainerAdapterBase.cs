using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public abstract class WidgetContainerAdapterBase : IWidgetContainerAdapter
    {
        public static IWidgetContainer GetWidgetContainer(DependencyObject obj)
        {
            return (IWidgetContainer)obj.GetValue(WidgetContainerProperty);
        }

        public static void SetWidgetContainer(DependencyObject obj, IWidgetContainer value)
        {
            obj.SetValue(WidgetContainerProperty, value);
        }

        // Using a DependencyProperty as the backing store for WidgetContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetContainerProperty =
            DependencyProperty.RegisterAttached("WidgetContainer", typeof(IWidgetContainer), typeof(WidgetContainerAdapterBase), null);

        protected IWidgetManager WidgetManager { get; set; }
        protected IWidgetContainerBehaviorFactory WidgetContainerBehaviorFactory { get; set; }
        protected IWidgetContainerAccessor ContainerAccessor { get; set; }

        protected WidgetContainerAdapterBase(
            IWidgetManager widgetManager,
            IWidgetContainerBehaviorFactory widgetContainerBehaviorFactory,
            IWidgetContainerAccessor containerAccessor)
        {
            WidgetManager = widgetManager;
            WidgetContainerBehaviorFactory = widgetContainerBehaviorFactory;
            ContainerAccessor = containerAccessor;
        }

        public void Adapt(FrameworkElement target)
        {
            if(target == null) throw new ArgumentNullException("target");

            IWidgetContainer container = CreateContainer();
            container.Target = target;
            InnerAdapt(container);
            AttachBehaviors(container);
            AttachDefaultBehaviors(container);
            ContainerAccessor.Container = container;
        }

        protected void AttachDefaultBehaviors(IWidgetContainer container)
        {
            foreach (string behaviorKey in WidgetContainerBehaviorFactory)
            {
                IWidgetContainerBehavior behavior = WidgetContainerBehaviorFactory.CreateFromKey(behaviorKey);
                behavior.Target = container;
                container.Behaviors.Add(behavior);
            }
        }

        protected virtual void AttachBehaviors(IWidgetContainer container) { }

        protected abstract void InnerAdapt(IWidgetContainer container);

        protected abstract IWidgetContainer CreateContainer();
    }
}
