using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;

namespace Gymnastika.Widgets
{
    public class ScatterViewWidgetContainerAdapter : WidgetContainerAdapterBase
    {
        private ScatterView _scatterView;

        public ScatterViewWidgetContainerAdapter(
            IWidgetManager widgetManager,
            IWidgetContainerBehaviorFactory widgetContainerBehaviorFactory,
            IWidgetContainerAccessor containerAccessor)
            :base(widgetManager, widgetContainerBehaviorFactory, containerAccessor)
        {
            
        }

        protected override void InnerAdapt(IWidgetContainer container)
        {
            _scatterView = (ScatterView)container.Target;
            container.WidgetHosts.CollectionChanged += WidgetHosts_CollectionChanged;
        }

        private void WidgetHosts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IWidgetHost host in e.NewItems)
                {
                    WidgetDescriptor descriptor = GetDescriptor(host.Widget);
                    ScatterViewItem viewItem = (ScatterViewItem) host;
                    viewItem.Center = descriptor.Position;
                    _scatterView.Items.Add(host);
                }
            }

            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var newItem in e.OldItems)
                {
                    _scatterView.Items.Remove(newItem);
                }
            }
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
