using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets.Behaviors
{
    public abstract class WidgetContainerBehaviorBase : IWidgetContainerBehavior
    {
        public void Attach()
        {
            if(Target == null) throw new InvalidOperationException("Target is null");
            IsAttached = true;
            OnAttach();
        }

        private IWidgetContainer _target;

        public IWidgetContainer Target
        {
            get { return _target; }
            set
            {
                if(IsAttached)
                    throw new InvalidOperationException(
                        string.Format("Behavior: {0} has already attached", this.GetType().Name));

                _target = value;
            }
        }

        protected bool IsAttached { get; private set; }

        protected abstract void OnAttach();
    }
}
