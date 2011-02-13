using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data
{
    public class WorkEnvironment : IWorkEnvironment
    {
        #region IWorkEnvironment Members

        public IWorkContextScope CreateWorkContextScope()
        {
            return WorkContextScope.Current;
        }

        #endregion
    }
}
