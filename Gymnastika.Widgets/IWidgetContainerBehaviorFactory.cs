using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IWidgetContainerBehaviorFactory : IEnumerable<string>
    {
        IWidgetContainerBehaviorFactory Register(string key, Type behaviorType);
        IWidgetContainerBehavior CreateFromKey(string key);
    }
}
