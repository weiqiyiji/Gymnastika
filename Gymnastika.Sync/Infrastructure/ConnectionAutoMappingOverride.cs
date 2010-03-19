using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gymnastika.Sync.Models;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping;

namespace Gymnastika.Sync.Infrastructure
{
    public class ConnectionAutoMappingOverride : IAutoMappingOverride<Connection>
    {
        #region IAutoMappingOverride<Connection> Members

        public void Override(AutoMapping<Connection> mapping)
        {
        }

        #endregion
    }
}