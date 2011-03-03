using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Gymnastika.Widgets
{
    public class WidgetContainerAccessor : IWidgetContainerAccessor
    {
        private IWidgetContainer _container;

        public IWidgetContainer Container
        {
            get { return _container; }
            set
            {
                if (_container != null)
                    throw new InvalidOperationException("Duplicate initialize WidgetContainer");

                if(value != null)
                {
                    _container = value;
                    RaiseContainerReady();
                    return;
                }

                throw new ArgumentNullException("WidgetContainer null on IWidgetContainerAccessor");
            }
        }

        private void RaiseContainerReady()
        {
            if (ContainerReady != null)
                ContainerReady(this, EventArgs.Empty);
        }

        #region IWidgetContainerAccessor Members

        public event EventHandler ContainerReady;

        #endregion
    }
}
