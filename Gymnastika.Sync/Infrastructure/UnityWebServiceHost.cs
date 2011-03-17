using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.ServiceModel.Description;

namespace Gymnastika.Sync.Infrastructure
{
    public class UnityWebServiceHost : WebServiceHost
    {
        public UnityWebServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        { }

        protected override void OnOpening()
        {
            base.OnOpening();

            if (Description.Behaviors.Find<UnityServiceBehavior>() == null)
                Description.Behaviors.Add(
                    new UnityServiceBehavior(ServiceLocator.Current.GetInstance<IUnityContainer>()));

            foreach (var endpoint in Description.Endpoints)
            {
                endpoint.Behaviors.Add(new SessionPerRequestBehavior());
            }
        }
    }
}