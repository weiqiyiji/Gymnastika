using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Gymnastika.Widgets.Infrastructure;

namespace Gymnastika.Widgets
{
    public class CanvasWidgetContainerAdapter : WidgetContainerAdapterBase
    {
        private Canvas _canvas;

        public CanvasWidgetContainerAdapter(
            IWidgetContainerBehaviorFactory widgetContainerBehaviorFactory, 
            IWidgetContainerAccessor containerAccessor) 
            : base(widgetContainerBehaviorFactory, containerAccessor)
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
                    if(widgetHost.IsActive)
                    {
                        Arrange(widgetHost);
                    }

                    widgetHost.IsActiveChanged += OnWidgetHostIsActiveChanged;
                    //TODO
                }
            }
        }

        private void OnWidgetHostIsActiveChanged(object sender, EventArgs e)
        {
            IWidgetHost widget = (IWidgetHost)sender;

            if(widget.IsActive)
            {
                Arrange(widget);
            }
            else
            {
                _canvas.Children.Remove(widget as UIElement);
            }
        }

        protected void Arrange(IWidgetHost widgetHost)
        {
            IPositionAware positionAware = widgetHost.Widget as IPositionAware;
            double x = 0.0, y = 0.0;
            int z = 0;

            if (positionAware != null)
            {
                x = positionAware.Position.X;
                y = positionAware.Position.Y;
                z = positionAware.ZIndex;
            }

            UIElement element = widgetHost as UIElement;

            if (element == null)
            {
                throw new InvalidCastException();
            }

            Canvas.SetTop(element, x);
            Canvas.SetLeft(element, y);
            Panel.SetZIndex(element, z);
            _canvas.Children.Add(element);
        }

        protected override IWidgetContainer CreateContainer()
        {
            return new WidgetContainer();
        }
    }
}
