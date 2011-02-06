using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Gymnastika.Data.SessionManagement
{
    public interface ISessionFactoryHolder
    {
        ISessionFactory GetSessionFactory();
        NHibernate.Cfg.Configuration GetConfiguration();
    }
}
