using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Gymnastika.Data.SessionManagement
{
    public interface ISessionLocator
    {
        ISession For(Type entityType);
        void CloseSession(Type entityType);
    }
}
