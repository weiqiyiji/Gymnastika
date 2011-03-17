using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Data;

namespace Gymnastika.Sync.Infrastructure
{
    public class WebEnvironment : IWorkEnvironment
    {
        #region IWorkEnvironment Members

        public IWorkContextScope GetWorkContextScope()
        {
            return WebContextScope.Current;
        }

        #endregion
    }
}