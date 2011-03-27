using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Data;
using System.Windows;

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

        public static WidgetDescriptor GetDescriptor(DependencyObject obj)
        {
            return (WidgetDescriptor)obj.GetValue(DescriptorProperty);
        }

        public static void SetDescriptor(DependencyObject obj, WidgetDescriptor value)
        {
            obj.SetValue(DescriptorProperty, value);
        }

        // Using a DependencyProperty as the backing store for Descriptor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptorProperty =
            DependencyProperty.RegisterAttached("Descriptor", typeof(WidgetDescriptor), typeof(ScatterViewWidgetContainerAdapter), null);    

        private void WidgetHosts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IWidgetHost host in e.NewItems)
                {
                    WidgetDescriptor descriptor = GetDescriptor(host.Widget);
                    ScatterViewItem viewItem = (ScatterViewItem) host;
                    Point position = descriptor.Position;

                    Binding binding = new Binding("Position");
                    binding.Source = descriptor;
                    binding.Mode = BindingMode.OneWayToSource;
                    viewItem.SetBinding(ScatterViewItem.CenterProperty, binding);
                    _scatterView.Items.Add(host);

                    viewItem.Center = position;
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
