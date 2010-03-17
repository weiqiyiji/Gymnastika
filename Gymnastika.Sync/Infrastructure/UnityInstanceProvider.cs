using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.Unity;

namespace Gymnastika.Sync.Infrastructure
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private Type _serviceType;
        private IUnityContainer _container;

        public UnityInstanceProvider(Type serviceType, IUnityContainer container)
        {
            _serviceType = serviceType;
            _container = container;
        }

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _container.Resolve(_serviceType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }

        #endregion
    }
}