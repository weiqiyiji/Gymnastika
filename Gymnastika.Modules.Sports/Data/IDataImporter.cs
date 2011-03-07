using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Data
{
    public interface IDataImporter<T>
    {
        bool NeedImport();
        void ImportData(IDataSource<T> source);
    }
}
