using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Gymnastika.Widgets.Infrastructure;

namespace Gymnastika.Widgets.Behaviors
{
    public class CreateWidgetHostBehavior : WidgetContainerBehaviorBase
    {
        private readonly IWidgetHostFactory _hostFactory;

        public const string BehaviorKey = "CreateWidgetHostBehavior";

        public CreateWidgetHostBehavior(IWidgetHostFactory hostFactory)
        {
            _hostFactory = hostFactory;
        }

        protected override void OnAttach()
        {
            Target.Widgets.CollectionChanged += OnWidgetsChanged;
        }

        private void OnWidgetsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IWidget widget in e.NewItems)
                {
                    widget.Initialize();
                    IWidgetHost widgetHost = _hostFactory.CreateWidgetHost();
                    widgetHost.Widget = widget;

                    IWidgetHostAware hostAware = widget as IWidgetHostAware;
                    if (hostAware != null)
                    {
                        hostAware.Host = widgetHost;
                    }

                    Target.WidgetHosts.Add(widgetHost);
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IWidget widget in e.OldItems)
                {
                    IWidgetHost host = Target.WidgetHosts.SingleOrDefault(x => x.Widget == widget);
                    if(host == null)
                    {
                        throw new InvalidOperationException("widget doesn't have a host");
                    }

                    Target.WidgetHosts.Remove(host);
                }
            }
        }
    }
}
