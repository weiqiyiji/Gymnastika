using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Data;
using System.ServiceModel.Web;

namespace Gymnastika.Sync.Infrastructure
{
    public class SessionPerRequestCallContextInitializer : ICallContextInitializer
    {
        private const string ContextScopeKey = "WorkContextScope";

        #region ICallContextInitializer Members

        public void AfterInvoke(object correlationState)
        {
            IWorkContextScope contextScope = (IWorkContextScope)correlationState;

            contextScope.Dispose();
        }

        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
        {
            IWorkEnvironment workEnvironment = ServiceLocator.Current.GetInstance<IWorkEnvironment>();
            return workEnvironment.GetWorkContextScope();
        }

        #endregion
    }
}