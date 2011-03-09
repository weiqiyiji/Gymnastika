using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.DataImport.Sources
{
    public interface IDataSource<T>
    {
        bool CanGetData();
        IEnumerable<T> GetData();
    }
}
