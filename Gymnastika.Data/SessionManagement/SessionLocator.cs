using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Logging;
using NHibernate;
using NHibernate.Type;
using System.Collections;
using NHibernate.SqlCommand;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Data.SessionManagement
{
    public class SessionLocator : ISessionLocator
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private readonly IWorkEnvironment _workEnvironment;
        private const string ContextKey = "CurrentSession";
        private const string TransactionKey = "TransactionScope";

        public SessionLocator(
            ISessionFactoryHolder sessionFactoryHolder,
            IWorkEnvironment workEnvironment,
            ILogger logger)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
            _workEnvironment = workEnvironment;
            Logger = logger;
        }

        public ILogger Logger { get; set; }

        public ISession For(Type entityType)
        {
            Logger.Debug("Acquiring session for {0}", entityType);

            IWorkContextScope currentScope = _workEnvironment.GetWorkContextScope();
            object session = null;

            if (!currentScope.Items.TryGetValue(ContextKey, out session))
            {
                currentScope.Disposing += WorkUnitScope_Disposing;

                var sessionFactory = _sessionFactoryHolder.GetSessionFactory();

                ITransactionManager txManager = ServiceLocator.Current.GetInstance<ITransactionManager>();
                txManager.Demand();

                currentScope.Items.Add(TransactionKey, txManager);

                Logger.Debug("Openning database session");
                session = sessionFactory.OpenSession();

                currentScope.Items.Add(ContextKey, session);
            }

            return session as ISession;
        }

        private void WorkUnitScope_Disposing(object sender, EventArgs e)
        {
            IWorkContextScope scope = (IWorkContextScope)sender;
            ITransactionManager txManager = scope.Items[TransactionKey] as ITransactionManager;
            txManager.Dispose();

            ISession session = scope.Items[ContextKey] as ISession;
            session.Close();
        }
    }
}
