using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Gymnastika.Widgets
{
    public class WidgetContainerBehaviorFactory : IWidgetContainerBehaviorFactory
    {
        private readonly IUnityContainer _container;
        private IDictionary<string, Type> _registeredBehaviors;

        public WidgetContainerBehaviorFactory(IUnityContainer container)
        {
            _container = container;
            _registeredBehaviors = new Dictionary<string, Type>();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _registeredBehaviors.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Register(string key, Type behaviorType)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (behaviorType == null)
            {
                throw new ArgumentNullException("behaviorType");
            }

            if (!typeof(IWidgetContainerBehavior).IsAssignableFrom(behaviorType))
            {
                throw new ArgumentException("Behavior can not assign to IWidgetContainerBehavior");
            }

            if (this._registeredBehaviors.ContainsKey(key))
            {
                return;
            }

            this._container.RegisterType(behaviorType);
            this._registeredBehaviors.Add(key, behaviorType);
        }

        public IWidgetContainerBehavior CreateFromKey(string key)
        {
            if (!_registeredBehaviors.ContainsKey(key))
                throw new KeyNotFoundException(key);

            return (IWidgetContainerBehavior)_container.Resolve(_registeredBehaviors[key]);
        }
    }
}
