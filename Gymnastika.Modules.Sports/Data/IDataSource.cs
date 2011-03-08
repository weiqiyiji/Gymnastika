using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Data
{
    public interface IDataSource<T>
    {
        bool CanGetData();
        IEnumerable<T> GetData();
    }
}
