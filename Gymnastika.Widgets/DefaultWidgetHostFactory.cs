using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets
{
    public class DefaultWidgetHostFactory : IWidgetHostFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public DefaultWidgetHostFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IWidgetHost CreateWidgetHost()
        {
            return _serviceLocator.GetInstance<IWidgetHost>();
        }
    }
}
