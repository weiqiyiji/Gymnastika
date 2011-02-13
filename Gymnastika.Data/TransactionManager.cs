using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Gymnastika.Common.Logging;

namespace Gymnastika.Data
{
    public interface ITransactionManager : IDisposable
    {
        void Demand();
        void Cancel();
    }

    public class TransactionManager : ITransactionManager
    {
        private TransactionScope _scope;
        private bool _cancelled;

        public TransactionManager(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; set; }

        public void Demand()
        {
            if (_scope == null)
            {
                Logger.Debug("Creating transaction on Demand");
                _scope = new TransactionScope(TransactionScopeOption.Required);
            }
        }

        public void Cancel()
        {
            Logger.Debug("Transaction cancelled flag set");
            _cancelled = true;
        }

        public void Dispose()
        {
            if (_scope != null)
            {
                if (!_cancelled)
                {
                    Logger.Debug("Marking transaction as complete");
                    _scope.Complete();
                }

                Logger.Debug("Final work for transaction being performed");
                _scope.Dispose();
                Logger.Debug("Transaction disposed");
            }
        }

    }
}
