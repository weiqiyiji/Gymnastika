using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
using System.ServiceModel.Dispatcher;

namespace Gymnastika.Sync.Infrastructure
{
    public class UnityServiceBehavior : IServiceBehavior
    {
        private IUnityContainer _container;

        public UnityServiceBehavior(IUnityContainer container)
        {
            _container = container;
        }

        #region IServiceBehavior Members

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            System.ServiceModel.ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(
            ServiceDescription serviceDescription, 
            System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase dispatcher in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = dispatcher as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (EndpointDispatcher endpoint in cd.Endpoints)
                    {
                        endpoint.DispatchRuntime.InstanceProvider =
                            new UnityInstanceProvider(serviceDescription.ServiceType, _container);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}