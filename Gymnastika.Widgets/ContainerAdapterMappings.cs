using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public class ContainerAdapterMappings
    {
        private readonly IDictionary<Type, IWidgetContainerAdapter> _mappings;

        public ContainerAdapterMappings()
        {
            _mappings = new Dictionary<Type, IWidgetContainerAdapter>();
        }

        public IWidgetContainerAdapter GetAdapter(Type containerType)
        {
            Type currentType = containerType;

            while (currentType != null)
            {
                if (_mappings.ContainsKey(currentType))
                {
                    return _mappings[currentType];
                }
                currentType = currentType.BaseType;
            }
            throw new KeyNotFoundException("containerType");
        }

        public void RegisterMapping(Type containerType, IWidgetContainerAdapter adapter)
        {
            if (containerType == null) throw new ArgumentNullException("containerType");
            if (adapter == null) throw new ArgumentNullException("adapter");
            if (_mappings.ContainsKey(containerType)) throw new InvalidOperationException("Container adapter mapping alread exists");
            
            _mappings.Add(containerType, adapter);
        }
    }
}
