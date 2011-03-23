using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Sync.Communication.Client
{
    public class ConnectionStore
    {
        public bool IsRegistered { get; private set; }
        public bool IsConnectionEstablished { get; private set; }
        public int AssignedId { get; private set; }
        public int ConnectionId { get; private set; }

        public void SaveConnection(int connectionId)
        {
            ConnectionId = connectionId;
            IsConnectionEstablished = true;
        }

        public void SaveAssignedInfo(int id)
        {
            AssignedId = id;
            IsRegistered = true;
        }
    }
}
