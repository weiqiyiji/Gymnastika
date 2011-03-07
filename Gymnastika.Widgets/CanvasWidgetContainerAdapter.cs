using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Gymnastika.Widgets
{
    public class CanvasWidgetContainerAdapter : WidgetContainerAdapterBase
    {
        private Canvas _canvas;

        public CanvasWidgetContainerAdapter(
            IWidgetManager widgetManager,
            IWidgetContainerBehaviorFactory widgetContainerBehaviorFactory, 
            IWidgetContainerAccessor containerAccessor)
            : base(widgetManager, widgetContainerBehaviorFactory, containerAccessor)
        {
        }

        protected override void InnerAdapt(IWidgetContainer container)
        {
            _canvas = container.Target as Canvas;
            if(_canvas == null)
            {
                throw new Exception("WidgetContainerAdapter mapping mismatch");
            }

            container.WidgetHosts.CollectionChanged += OnWidgetHostsChanged;
        }

        private void OnWidgetHostsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IWidgetHost widgetHost in e.NewItems)
                {
                     Arrange(widgetHost);
                }
            }
            
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IWidgetHost widgetHost in e.OldItems)
                {
                    _canvas.Children.Remove(widgetHost as UIElement);
                }
            }
        }

        protected void Arrange(IWidgetHost widgetHost)
        {
            WidgetDescriptor descriptor = GetDescriptor(widgetHost.Widget);
            UIElement element = widgetHost as UIElement;

            if (element == null)
            {
                throw new InvalidCastException();
            }

            Canvas.SetTop(element, descriptor.Position.Y);
            Canvas.SetLeft(element, descriptor.Position.X);
            Panel.SetZIndex(element, descriptor.ZIndex);
            _canvas.Children.Add(element);
        }

        private WidgetDescriptor GetDescriptor(IWidget widget)
        {
            return WidgetManager.Descriptors.Single(x => x.WidgetType == widget.GetType());
        }

        protected override IWidgetContainer CreateContainer()
        {
            return new WidgetContainer();
        }
    }
}
