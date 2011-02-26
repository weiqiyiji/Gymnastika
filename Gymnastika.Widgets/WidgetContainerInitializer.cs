using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public class WidgetContainerInitializer : IWidgetContainerInitializer
    {
        private readonly ContainerAdapterMappings _adapterMappings;

        public WidgetContainerInitializer(ContainerAdapterMappings adapterMappings)
        {
            _adapterMappings = adapterMappings;
        }

        public void Initialize(FrameworkElement target)
        {
            IWidgetContainerAdapter adapter = _adapterMappings.GetAdapter(target.GetType());
            adapter.Adapt(target);
        }
    }
}
